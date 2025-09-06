namespace CustomsDED.Data.Models
{
    using CustomsDED.Common.Enums;

    public class Person
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public string? PersonalNumber { get; set; }

        public string? DocumentNumber { get; set; }

        public string? DocumentType { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string? IssuingCountry { get; set; }

        public SexType? SexType { get; set; }

        public string? AdditionInfo { get; set; }

        public string? DateOfInspection { get; set; }

    }
}
