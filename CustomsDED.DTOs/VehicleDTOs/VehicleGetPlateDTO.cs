namespace CustomsDED.DTOs.VehicleDTOs
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class VehicleGetPlateDTO : VehicleBaseDTO
    {
        [ObservableProperty]
        public DateTime? dateOfInspection;

    }
}
