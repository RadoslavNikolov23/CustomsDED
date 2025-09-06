namespace CustomsDED.DTOs.VehicleDTOs
{
    using System;

    public class VehicleAddDTO
    {
        public string LicensePlate { get; set; } = null!;

        public string? Info { get; set; }

        public DateTime? DateOfInspection { get; set; }
    }
}
