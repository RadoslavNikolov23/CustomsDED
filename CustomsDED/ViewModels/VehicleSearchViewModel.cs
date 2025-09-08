namespace CustomsDED.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CustomsDED.Data.Models;
    using CustomsDED.DTOs.VehicleDTOs;
    using CustomsDED.Services.VehicleServices.Contract;
    using System.Collections.ObjectModel;

    public partial class VehicleSearchViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string enterLicensePlateEntry = "";

        //[ObservableProperty]
        //private string licensePlateLabel = "";

        //[ObservableProperty]
        //private string additionInfoLabel = "";

        //[ObservableProperty]
        //private string dateOfInspectionLabel = "";

        [ObservableProperty]
        private ObservableCollection<VehicleGetPlateDTO> vehiclesResultList = new();

        private readonly IVehicleService vehicleService;

        public VehicleSearchViewModel(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }


        [RelayCommand]
        private async Task SearchVehicle()
        {
            string queryText = this.EnterLicensePlateEntry;

            ICollection<VehicleGetPlateDTO> vehicleList = await this.vehicleService
                                                                    .GetVehiclesByTextAsync(queryText);
            foreach (VehicleGetPlateDTO vehicle in vehicleList)
            {
                this.VehiclesResultList.Add(vehicle);
            }

        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
    
}
