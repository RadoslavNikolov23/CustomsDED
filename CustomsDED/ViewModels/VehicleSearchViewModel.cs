namespace CustomsDED.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using CustomsDED.Common.Helpers;
    using CustomsDED.DTOs.VehicleDTOs;
    using CustomsDED.Resources.Localization;
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

                if (vehicleList != null && vehicleList.Count > 0)
                {
                    this.VehiclesResultList.Clear();

                    foreach (VehicleGetPlateDTO vehicle in vehicleList)
                    {
                        VehiclesResultList.Add(vehicle);
                    }
                }
                else
                {
                    await ShowPopupMessage(AppResources.Information,
                                           AppResources.NoVehiclesFoundWithTheProvidedInfo);
                }

                this.EnterLicensePlateEntry = "";
            }
            catch (Exception ex)
            {
                this.VehiclesResultList.Clear();
                this.EnterLicensePlateEntry = "";
                await Logger.LogAsync(ex, "Error in SearchVehicle, in the VehicleSearchViewModel class");
                await ShowPopupMessage(AppResources.Error,
                                       AppResources.AnErrorOccurredWhileSearchingVehicle);
            }
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
