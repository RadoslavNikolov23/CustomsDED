namespace CustomsDED.Data.Models
{
    using SQLite;

    public class Vehicle
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [MaxLength(10)]
        public string LicensePlate { get; set; } = null!;

        [MaxLength(2045)]
        public string? AdditionalInfo { get; set; }

        public DateTime? DateOfInspection { get; set; }
    }
}
