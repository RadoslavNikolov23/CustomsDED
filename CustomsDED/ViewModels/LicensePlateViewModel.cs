namespace CustomsDED.ViewModels
{
    using System.Text.RegularExpressions;
    using Plugin.Maui.OCR;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Services.VehicleServices.Contract;
    using CustomsDED.DTOs.VehicleDTOs;
    using CustomsDED.Common.Helpers;
    using CustomsDED.Resources.Localization;

    using static CustomsDED.Services.CropImageServices.CropImageService;
    using static CustomsDED.Common.Regex.RegexLicensePlates;

    public partial class LicensePlateViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string plateLicenseEntry = "";

        [ObservableProperty]
        private string additionInfoEntry = "";

        private readonly IOcrService ocrService;

        private readonly IVehicleService vehicleService;

        public LicensePlateViewModel(IOcrService ocrService, IVehicleService vehicleService)
        {
            this.ocrService = ocrService;
            this.vehicleService = vehicleService;
        }

        [RelayCommand]
        private async Task Clear()
        {
            this.PlateLicenseEntry = "";
        }

        public async Task<string[]> ProcessCapturedImageAsync(byte[] photoBytes)
        {
            string[] textMessage = new string[2];

            byte[]? croppedBytes = await CropToOverlayAsync(photoBytes);
            
            if (croppedBytes == null)
            {
                textMessage[0] = AppResources.Error;
                textMessage[1] = AppResources.ScanDocumentFailedPleaseTryAgain;

                return textMessage;
            }

            OcrResult result = await this.ocrService.RecognizeTextAsync(croppedBytes, tryHard: true);

            if (!result.Success)
            {
                textMessage[0] = AppResources.Error;
                textMessage[1] = AppResources.ScanDocumentFailedPleaseTryAgain;

                return textMessage;
            }


            try
            {
                string? plate = await ExtractLicensePlate(result.AllText);

                if (plate == null)
                {
                    if (result.AllText != null)
                    {
                        this.PlateLicenseEntry = result.AllText;
                        textMessage[0] = AppResources.Error;
                        textMessage[1] = AppResources.NoValidLicensePlateFoundTryAgainOrFixManual;
                    }
                    else
                    {
                        textMessage[0] = AppResources.Error;
                        textMessage[1] = AppResources.PictureWasTakenThereWereNoCapturedResultsTryAgain;
                    }

                }
                else
                {
                    textMessage[0] = AppResources.Information;
                    textMessage[1] = AppResources.PictureTakenSeeResult;

                    this.PlateLicenseEntry = plate;
                }

                return textMessage;
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in ProcessCapturedImageAsync, in the LicensePlateViewModel class.");
                throw;
            }
        }

        [RelayCommand]
        private async Task SaveVehicle()
        {
            try
            {
                if (string.IsNullOrEmpty(this.PlateLicenseEntry))
                {
                    await ShowPopupMessage(AppResources.Error,
                                           AppResources.EnterVehicleLicensePlateFirst);
                    return;
                }

                string? cleanedPlate = FilterInput(this.PlateLicenseEntry);

                if (cleanedPlate == null)
                {
                    await ShowPopupMessage(AppResources.Error,
                                           AppResources.FailedToSaveVehiclePleaseTryAgain);
                    return;
                }

                bool isSaved = await this.vehicleService
                                                .AddVehicleAsync(new VehicleAddDTO
                                                {
                                                    LicensePlate = cleanedPlate,
                                                    AdditionalInfo = this.AdditionInfoEntry
                                                });

                if (isSaved)
                {
                    ClearEntries();
                    await ShowPopupMessage(AppResources.Success,
                                           AppResources.VehicleSavedSuccessfully);
                }
                else
                {
                    await ShowPopupMessage(AppResources.Error,
                                           AppResources.FailedToSaveVehiclePleaseTryAgain);
                }
            }
            catch (Exception ex)
            {
                ClearEntries();
                await Logger.LogAsync(ex, "Error in SaveVehicle, in the LicensePlateViewModel class.");
                await ShowPopupMessage(AppResources.Error,
                                       AppResources.SomethingFailedPleaseTryAgainContactDevelepors);
            }
        }

        private void ClearEntries()
        {
            this.PlateLicenseEntry = "";
            this.AdditionInfoEntry = "";
        }

        private async Task<string?> ExtractLicensePlate(string ocrText)
        {
            await Task.Yield();

            if (string.IsNullOrWhiteSpace(ocrText))
                return null;

            string normalized = Regex.Replace(ocrText.ToUpper(), @"\s+", " ").Trim();

            List<string> tokens = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            //string[] stopWords = { "SERVICE", "TOYOTA", "FORD", "CITY", "RUSE" };
            //tokens.RemoveAll(t => stopWords.Contains(t));

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "BG" || tokens[i] == "RO" || tokens[i] == "TR")
                {
                    tokens[i] = string.Empty;
                    continue;
                }

                tokens[i] = Regex.Replace(tokens[i], @"^(BG|RO|TR)", "");
            }

            tokens = tokens.Where(t => !string.IsNullOrEmpty(t)).ToList();

            foreach (var token in tokens)
            {
                if (BulgarianRegex.IsMatch(token)) return token;
                if (RomanianRegex.IsMatch(token)) return token;
                if (TurkishRegex.IsMatch(token)) return token;
            }

            string glued = string.Join("", tokens);
            MatchCollection matches = Regex.Matches(glued, @"[A-Z0-9]{5,10}"); // typical plate length
            
            foreach (Match match in matches)
            {
                string candidate = match.Value;

                if (BulgarianRegex.IsMatch(candidate)) return candidate;
                if (RomanianRegex.IsMatch(candidate)) return candidate;
                if (TurkishRegex.IsMatch(candidate)) return candidate;
            }

            return null;
        }

      

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
