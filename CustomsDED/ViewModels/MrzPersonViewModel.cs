namespace CustomsDED.ViewModels
{
    using Plugin.Maui.OCR;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Common.Enums;
    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Models;
    using CustomsDED.DTOs.PersonDTOs;
    using CustomsDED.Services.MrzParsesServices;
    using CustomsDED.Services.PersonServices.Contract;

    using static CustomsDED.Services.CropImageServices.CropImageService;


    public partial class MrzPersonViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isCameraViewVisible = false;

        [ObservableProperty]
        private bool isCameraViewLayotVisible = false;

        [ObservableProperty]
        private string firstNameEntry = "";

        [ObservableProperty]
        private string middleNameEntry = "";

        [ObservableProperty]
        private bool isMiddleNameVisibleEntry = false;

        [ObservableProperty]
        private string lastNameEntry = "";

        [ObservableProperty]
        private string dateOfBirthEntry = "";

        [ObservableProperty]
        private string nationalityEntry = "";

        [ObservableProperty]
        private string personalIdEntry = "";

        [ObservableProperty]
        private string sexEntry = "";

        [ObservableProperty]
        private string documentTypeEntry = "";

        [ObservableProperty]
        private string documentNumberEntry = "";

        [ObservableProperty]
        private string expirDateEntry = "";

        [ObservableProperty]
        private string issuingContryEntry = "";

        [ObservableProperty]
        private string additionInfoEntry = "";

        private readonly IOcrService ocrService;

        private readonly IPersonService personService;


        public MrzPersonViewModel(IOcrService ocrService, IPersonService personService)
        {
            this.ocrService = ocrService;
            this.personService = personService;
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
            OcrResult result = await this.ocrService.RecognizeTextAsync(croppedBytes,tryHard: true);

            if (!result.Success)
            {
                //TODO: fix the message!
                await ShowPopupMessage("Error", "OCR failed. Please try again.");
                return;
            }

            // Parse MRZ
            try
            {
                string[] mrzLines = result.AllText
                                     .Split('\n')
                                     .Select(l => l.Trim())
                                     .Where(l => !string.IsNullOrWhiteSpace(l) && l.Length >= 30)
                                     .ToArray();

                Person? infoPerson = await MrzParserService.ParseMRZCodeAsync(mrzLines);

                if (infoPerson == null)
                {
                    //TODO: fix the message!
                    await ShowPopupMessage("Error", "Something failed. Please try again.");
                    return;
                }

                this.FirstNameEntry = infoPerson.FirstName;
                if (infoPerson.MiddleName != null)
                {
                    this.MiddleNameEntry = infoPerson.MiddleName;
                    this.IsMiddleNameVisibleEntry = true;
                }
                else
                {
                    this.IsMiddleNameVisibleEntry = false;
                }
                this.LastNameEntry = infoPerson.LastName;

                this.DateOfBirthEntry = infoPerson.DateOfBirth != null ? 
                                                        $"{infoPerson.DateOfBirth:yyyy-MM-dd}" : "";
                this.NationalityEntry = infoPerson.Nationality ?? "";

                this.PersonalIdEntry = infoPerson.PersonalNumber ?? "";
                this.SexEntry = infoPerson.SexType.ToString() ?? "";

                this.DocumentTypeEntry = infoPerson.DocumentType ?? "";
                this.DocumentNumberEntry = infoPerson.DocumentNumber ?? "";
                this.ExpirDateEntry = infoPerson.ExpirationDate != null ? 
                                                        $"{infoPerson.ExpirationDate:yyyy-MM-dd}" : "";
                this.IssuingContryEntry = infoPerson.IssuingCountry ?? "";

                this.AdditionInfoEntry = infoPerson.AdditionInfo ?? "";

            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in ProcessCapturedImageAsync, in the MrzPersonViewModel class.");
                await ShowPopupMessage("Error", "An error occurred while saving the person. Please try again.");
            }
        }


        [RelayCommand]
        private async Task OpenCameraView()
        {
            this.IsCameraViewVisible = true;
            this.IsCameraViewLayotVisible = true;
        }

        [RelayCommand]
        private async Task CloseCameraView()
        {
            this.IsCameraViewVisible = false;
            this.IsCameraViewLayotVisible = false;
        }

        [RelayCommand]
        private async Task SavePerson()
        {
            DateTime? personDOB = MrzParserService.ParseDOBDate(this.DateOfBirthEntry);
            DateTime? personExpDate = MrzParserService.ParseExpirationDate(this.ExpirDateEntry);

            bool isValidSex = Enum.TryParse<SexType>(this.SexEntry, out SexType sexEnum);

            try
            {
                bool isSaved = await this.personService
                                            .AddPersonAsync(new PersonAddDTO
                                            {
                                                FirstName = this.FirstNameEntry,
                                                MiddleName = this.MiddleNameEntry,
                                                LastName = this.LastNameEntry,
                                                DateOfBirth = personDOB ?? null,
                                                Nationality = this.NationalityEntry,
                                                PersonalId = this.PersonalIdEntry,
                                                SexType = isValidSex ? sexEnum : null,
                                                DocumentType = this.DocumentTypeEntry,
                                                DocumentNumber = this.DocumentNumberEntry,
                                                ExpirationDate = personExpDate ?? null,
                                                IssuingCountry = this.IssuingContryEntry,
                                                AdditionInfo = this.AdditionInfoEntry,
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
                await Logger.LogAsync(ex, "Error in SavePerson, in the MrzPersonViewModel class.");
                await ShowPopupMessage("Error", "An error occurred while saving the person. Please try again.");
            }
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
