namespace CustomsDED.Data.Models
{
    public class Vehicle
    {
        public long Id { get; set; }

        public string LicensePlate { get; set; } = null!;

        public string? Info { get; set; }

        public string? DateOfInspection { get; set; }
    }
}
