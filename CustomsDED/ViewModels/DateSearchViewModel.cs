namespace CustomsDED.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Common.Helpers;
    using CustomsDED.DTOs.PersonDTOs;
    using CustomsDED.DTOs.VehicleDTOs;
    using CustomsDED.Resources.Localization;
    using CustomsDED.Services.PersonServices.Contract;
    using CustomsDED.Services.VehicleServices.Contract;

    public partial class DateSearchViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime inspectionDatePicker;

        [ObservableProperty]
        private ObservableCollection<PersonGetDateDTO> datePersonResultList = new();

        [ObservableProperty]
        private ObservableCollection<VehicleGetDateDTO> dateVehiclesResultList = new();


        private readonly IPersonService personService;

        private readonly IVehicleService vehicleService;

        public DateSearchViewModel(IPersonService personService, IVehicleService vehicleService)
        {
            this.personService = personService;
            this.vehicleService = vehicleService;
        }
        
        [RelayCommand]
        private async Task SearchByDate()
        {
            DateTime datePicked = this.InspectionDatePicker;

            try
            {
                ICollection<PersonGetDateDTO> personList = await this.personService
                                                                        .GetPersonsByDateAsync(datePicked);

                ICollection<VehicleGetDateDTO> vehicleList = await this.vehicleService
                                                                        .GetVehiclesByDateAsync(datePicked);
                foreach (PersonGetDateDTO person in personList)
                {
                    this.DatePersonResultList.Add(person);
                }

                foreach (VehicleGetDateDTO vehicle in vehicleList)
                {
                    this.DateVehiclesResultList.Add(vehicle);
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in SearchByDate, in the DateSearchViewModel class");
                await ShowPopupMessage(AppResources.Error, 
                                       AppResources.AnErrorOccurredWhileSearching);
            }
        }
        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
