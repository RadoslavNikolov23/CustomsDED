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

        public async Task<string[]> ProcessCapturedImageAsync(byte[] photoBytes)
        {
            string[] textMessage = new string[2];

            byte[]? croppedBytes = await CropToOverlayAsync(photoBytes);
            
            if (croppedBytes == null)
            {
                // await ShowPopupMessage(AppResources.Error,
                //                       AppResources.ScanDocumentFailedPleaseTryAgain);

                textMessage[0] = AppResources.Error;
                textMessage[1] = AppResources.ScanDocumentFailedPleaseTryAgain;
                return textMessage;
            }

            OcrResult result = await this.ocrService.RecognizeTextAsync(croppedBytes, tryHard: true);

            if (!result.Success)
            {
                //await ShowPopupMessage(AppResources.Error,
                //                       AppResources.ScanDocumentFailedPleaseTryAgain);
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

                        //await ShowPopupMessage(AppResources.Error,
                        //                      AppResources.NoValidLicensePlateFoundTryAgainOrFixManual);
                        this.PlateLicenseEntry = result.AllText;
                        textMessage[0] = AppResources.Error;
                        textMessage[1] = AppResources.NoValidLicensePlateFoundTryAgainOrFixManual;
                    }
                    else
                    {
                        // await ShowPopupMessage(AppResources.Error,
                        //                       AppResources.PictureWasTakenThereWereNoCapturedResultsTryAgain);
                        textMessage[0] = AppResources.Error;
                        textMessage[1] = AppResources.PictureWasTakenThereWereNoCapturedResultsTryAgain;
                    }

                }
                else
                {
                    //await ShowPopupMessage(AppResources.Information,
                    //                       AppResources.PictureTakenSeeResult);
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

                string cleanedPlate = Regex.Replace(this.PlateLicenseEntry.ToUpper(), @"[^A-Z0-9]", "");

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
                                       AppResources.AnErrorOccurredWhileSavingVehicle);
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

            // Strip everything except A–Z and 0–9
            string cleanText = Regex.Replace(ocrText.ToUpper(), @"[^A-Z0-9]", "");

            // Remove country prefix if present
            cleanText = Regex.Replace(cleanText, @"^(BG|RO|TR)", "");

            // Candidates = continuous alphanumeric strings
            List<string> candidates = Regex.Matches(cleanText, @"[A-Z0-9]+")
                                           .Select(m => m.Value)
                                           .ToList();

            foreach (var candidate in candidates)
            {
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
