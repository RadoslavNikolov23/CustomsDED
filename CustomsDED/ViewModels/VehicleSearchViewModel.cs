namespace CustomsDED.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Common.Helpers;
    using CustomsDED.DTOs.VehicleDTOs;
    using CustomsDED.Services.VehicleServices.Contract;

    public partial class VehicleSearchViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string enterLicensePlateEntry = "";

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

            try
            {
                ICollection<VehicleGetPlateDTO> vehicleList = await this.vehicleService
                                                                        .GetVehiclesByTextAsync(queryText);
                foreach (VehicleGetPlateDTO vehicle in vehicleList)
                {
                    this.VehiclesResultList.Add(vehicle);
                }
            }
            catch (Exception ex)
            {
                await Logger.LogAsync(ex, "Error in SearchVehicle, in the VehicleSearchViewModel class");
                await ShowPopupMessage("Error", "An error occurred while searching for vehicles.");
            }
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }

}
