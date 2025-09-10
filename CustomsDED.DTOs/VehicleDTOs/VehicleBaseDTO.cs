namespace CustomsDED.DTOs.VehicleDTOs
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class VehicleBaseDTO : ObservableObject
    {
        [ObservableProperty]
        public string? licensePlate;

        [ObservableProperty]
        public string? additionalInfo;
    }
}
