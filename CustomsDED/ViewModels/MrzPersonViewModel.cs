namespace CustomsDED.ViewModels
{
    using Plugin.Maui.OCR;
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Common.Enums;
    using CustomsDED.Common.Helpers;
    using CustomsDED.Data.Models;
    using CustomsDED.DTOs.PersonDTOs;
    using CustomsDED.Resources.Localization;
    using CustomsDED.Services.MrzParsesServices;
    using CustomsDED.Services.PersonServices.Contract;

    using static CustomsDED.Services.CropImageServices.CropImageService;

    public partial class MrzPersonViewModel : BaseViewModel
    {
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
        private SexType? selectedSex;

        [ObservableProperty]
        private string documentTypeEntry = "";

        [ObservableProperty]
        private string documentNumberEntry = "";

        [ObservableProperty]
        private string expirDateEntry = "";

        [ObservableProperty]
        private string issuingCountryEntry = "";

        [ObservableProperty]
        private string additionInfoEntry = "";

        public ObservableCollection<SexType> SexOptions { get; } = new(Enum.GetValues<SexType>());

        private readonly IOcrService ocrService;

        private readonly IPersonService personService;


        public MrzPersonViewModel(IOcrService ocrService, IPersonService personService)
        {
            this.ocrService = ocrService;
            this.personService = personService;
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

            OcrResult result = await this.ocrService
                                        .RecognizeTextAsync(croppedBytes, tryHard: true);

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
                string[] mrzLines = result.AllText
                                     .Split('\n')
                                     .Select(l => l.Trim())
                                     .Where(l => !string.IsNullOrWhiteSpace(l) && l.Length >= 30)
                                     .ToArray();

                Person? infoPerson = await MrzParserService.ParseMRZCodeAsync(mrzLines);

                if (infoPerson == null)
                {
                    //await ShowPopupMessage(AppResources.Error,
                    //                       AppResources.SomethingFailedPleaseTryAgain);

                    textMessage[0] = AppResources.Error;
                    textMessage[1] = AppResources.SomethingFailedPleaseTryAgain;
                    return textMessage;
                }

                // await ShowPopupMessage(AppResources.Information,
                //                       AppResources.PictureTakenSeeResult);

                textMessage[0] = AppResources.Information;
                textMessage[1] = AppResources.PictureTakenSeeResult;


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

                this.PersonalIdEntry = infoPerson.PersonalId ?? "";
                this.SelectedSex = infoPerson.SexType;

                this.DocumentTypeEntry = infoPerson.DocumentType ?? "";
                this.DocumentNumberEntry = infoPerson.DocumentNumber ?? "";
                this.ExpirDateEntry = infoPerson.ExpirationDate != null ?
                                                        $"{infoPerson.ExpirationDate:yyyy-MM-dd}" : "";
                this.IssuingCountryEntry = infoPerson.IssuingCountry ?? "";

                this.AdditionInfoEntry = infoPerson.AdditionInfo ?? "";

                return textMessage;

            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in ProcessCapturedImageAsync, in the MrzPersonViewModel class.");
                throw;
            }
        }



        [RelayCommand]
        private async Task SavePerson()
        {
            DateTime? personDOB = MrzParserService.ParseDOBDate(this.DateOfBirthEntry);
            DateTime? personExpDate = MrzParserService.ParseExpirationDate(this.ExpirDateEntry);

            if (String.IsNullOrEmpty(this.FirstNameEntry) || String.IsNullOrEmpty(this.LastNameEntry))
            {
                await ShowPopupMessage(AppResources.Error,
                                       AppResources.FirstAndLastNameAreRequired);
                return;
            }

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
                                                SexType = this.SelectedSex,
                                                DocumentType = this.DocumentTypeEntry,
                                                DocumentNumber = this.DocumentNumberEntry,
                                                ExpirationDate = personExpDate ?? null,
                                                IssuingCountry = this.IssuingCountryEntry,
                                                AdditionInfo = this.AdditionInfoEntry,
                                            });

                if (isSaved)
                {
                    ClearEntries();
                    await ShowPopupMessage(AppResources.Success,
                                           AppResources.PersonSavedSuccessfully);
                }
                else
                {
                    await ShowPopupMessage(AppResources.Error,
                                           AppResources.FailedToSavePersonPleaseTryAgain);
                }
            }
            catch (Exception ex)
            {
                ClearEntries();
                await Logger.LogAsync(ex, "Error in SavePerson, in the MrzPersonViewModel class.");
                await ShowPopupMessage(AppResources.Error,
                                       AppResources.AnErrorOccurredWhileSavingPerson);
            }
        }

        public void ClearEntries()
        {
            this.FirstNameEntry = "";
            this.MiddleNameEntry = "";
            this.IsMiddleNameVisibleEntry = false;
            this.LastNameEntry = "";

            this.DateOfBirthEntry = "";
            this.NationalityEntry = "";

            this.PersonalIdEntry = "";
            this.SelectedSex = null;

            this.DocumentTypeEntry = "";
            this.DocumentNumberEntry = "";
            this.ExpirDateEntry = "";
            this.IssuingCountryEntry = "";

            this.AdditionInfoEntry = "";
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
