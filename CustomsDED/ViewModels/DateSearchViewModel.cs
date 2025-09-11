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
        private DateTime inspectionDatePicker = DateTime.Today;

        [ObservableProperty]
        private string isEmptyVehicleList = "";

        [ObservableProperty]
        private bool isEmptyVehicleListVisible = false;

        [ObservableProperty]
        private string isEmptyPersonList = "";

        [ObservableProperty]
        private bool isEmptyPersonListVisible = false;

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
                ICollection<VehicleGetDateDTO> vehicleList = await this.vehicleService
                                                                   .GetVehiclesByDateAsync(datePicked);

                if (vehicleList != null && vehicleList.Count > 0)
                {
                    this.DateVehiclesResultList.Clear();
                    this.IsEmptyVehicleListVisible = false;

                    foreach (VehicleGetDateDTO vehicle in vehicleList)
                    {
                        this.DateVehiclesResultList.Add(vehicle);
                    }
                }
                else
                {
                    this.DateVehiclesResultList.Clear();
                    string isEmptyVehicleList = String.Format(AppResources.NoVehiclesFoundForDate,  
                                                              datePicked.ToString("dd/MM/yyyy"));
                    this.IsEmptyVehicleListVisible = true;
                }

                ICollection<PersonGetDateDTO> personList = await this.personService
                                                                        .GetPersonsByDateAsync(datePicked);

                if (personList != null && personList.Count > 0)
                {
                    this.DatePersonResultList.Clear();
                    this.IsEmptyVehicleListVisible = false;


                    foreach (PersonGetDateDTO person in personList)
                    {
                        this.DatePersonResultList.Add(person);
                    }
                }
                else
                {
                    this.DatePersonResultList.Clear();

                    string isEmptyVehicleList = String.Format(AppResources.NoPersonsFoundForDate,
                                                              datePicked.ToString("dd/MM/yyyy"));
                    this.IsEmptyVehicleListVisible = true;
                }


            }
            catch (Exception ex)
            {
                this.DateVehiclesResultList.Clear();
                this.DatePersonResultList.Clear();

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
