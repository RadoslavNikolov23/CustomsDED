namespace CustomsDED.ViewModels
{
    using System.Text.RegularExpressions;
    using Plugin.Maui.OCR;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Services.VehicleServices.Contract;
    using CustomsDED.DTOs.VehicleDTOs;
    using CustomsDED.Common.Helpers;

    using static CustomsDED.Services.CropImageServices.CropImageService;
    using static CustomsDED.Common.Regex.RegexLicensePlates;

    public partial class LicensePlateViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isCameraViewVisible = false;

        [ObservableProperty]
        private bool isCameraViewLayotVisible = false;


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


        public async Task ProcessCapturedImageAsync(byte[] photoBytes)
        {
            // Crop
            byte[]? croppedBytes = await CropToOverlayAsync(photoBytes);

            if (croppedBytes == null)
            {
                //TODO : fix the message!
                await ShowPopupMessage("Error", "Image cropping failed. Please try again.");
                return;
            }

            // OCR
            OcrResult result = await this.ocrService.RecognizeTextAsync(croppedBytes, tryHard: true);

            if (!result.Success)
            {
                //TODO: fix the message!
                await ShowPopupMessage("Error", "OCR failed. Please try again.");
                return;
            }

            string? plate = ExtractLicensePlate(result.AllText);

            if (plate == null)
            {
                await ShowPopupMessage("Error", "No valid license plate found. Please try again or fix it manual!.");
                this.PlateLicenseEntry = $"{result.AllText}";
            }
            else
            {
                this.PlateLicenseEntry = plate ?? $"{result.AllText} Could correctly";
            }
        }

        [RelayCommand]
        private void OpenCameraView()
        {
            this.IsCameraViewVisible = true;
            this.IsCameraViewLayotVisible = true;
        }

        [RelayCommand]
        private void CloseCameraView()
        {
            this.IsCameraViewVisible = false;
            this.IsCameraViewLayotVisible = false;
        }

        [RelayCommand]
        private async Task SaveVehicle()
        {
            try
            {
                bool isSaved = await this.vehicleService
                                                .AddVehicleAsync(new VehicleAddDTO
                                                {
                                                    LicensePlate = this.PlateLicenseEntry,
                                                    AdditionalInfo = this.AdditionInfoEntry
                                                });

                if (isSaved)
                {
                    await ShowPopupMessage("Success", "Vehicle saved successfully.");
                }
                else
                {
                    await ShowPopupMessage("Error", "Failed to save vehicle. Please try again.");
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in SaveVehicle, in the LicensePlateViewModel class.");
                await ShowPopupMessage("Error", "An error occurred while saving the vehicle. Please try again.");
            }
        }

        private string? ExtractLicensePlate(string ocrText)
        {
            // Normalize OCR text
            string cleanText = ocrText
                                .ToUpper()
                                .Replace(" ", "")
                                .Replace("-", "")
                                .Replace("\n", "")
                                .Replace("\r", "");

            // Split into words in case OCR returns multiple chunks
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
