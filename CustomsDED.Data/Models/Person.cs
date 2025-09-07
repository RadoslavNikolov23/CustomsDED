namespace CustomsDED.Data.Models
{
    using CustomsDED.Common.Enums;
    using SQLite;

    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [MaxLength(65)]
        public string FirstName { get; set; } = null!;

        [MaxLength(65)]
        public string? MiddleName { get; set; }

        [MaxLength(65)]
        public string LastName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(5)]
        public string? Nationality { get; set; }

        [MaxLength(15)]
        public string? PersonalNumber { get; set; }

        public SexType? SexType { get; set; }

        [MaxLength(15)]
        public string? DocumentNumber { get; set; }

        [MaxLength(10)]
        public string? DocumentType { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [MaxLength(5)]
        public string? IssuingCountry { get; set; }

        [MaxLength(2045)]
        public string? AdditionInfo { get; set; }

        public DateTime? DateOfInspection { get; set; }

    }
}
