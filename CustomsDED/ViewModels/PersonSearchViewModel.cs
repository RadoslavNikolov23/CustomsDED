namespace CustomsDED.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Common.Helpers;
    using CustomsDED.DTOs.PersonDTOs;
    using CustomsDED.Resources.Localization;
    using CustomsDED.Services.PersonServices.Contract;

    public partial class PersonSearchViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string enterPersonInfoEntry = "";

        [ObservableProperty]
        private ObservableCollection<PersonGetTextDTO> personResultList = new();

        private readonly IPersonService personService;

        public PersonSearchViewModel(IPersonService personService)
        {
            this.personService = personService;
        }

        [RelayCommand]
        private async Task SearchPerson()
        {
            string queryText = this.EnterPersonInfoEntry;

            try
            {
                ICollection<PersonGetTextDTO> personList = await this.personService
                                                                        .GetPersonsByTextAsync(queryText);
                foreach (PersonGetTextDTO person in personList)
                {
                    this.PersonResultList.Add(person);
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in SearchPerson, in the PersonSearchViewModel class");
                await ShowPopupMessage(AppResources.Error, 
                                       AppResources.AnErrorOccurredWhileSearchingPerson);
            }
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
