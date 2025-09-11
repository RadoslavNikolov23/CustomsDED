namespace CustomsDED.DTOs.VehicleDTOs
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class VehicleGetDateDTO : VehicleBaseDTO
    {
        [ObservableProperty]
        public DateTime? dateOfInspection;

    }
}
