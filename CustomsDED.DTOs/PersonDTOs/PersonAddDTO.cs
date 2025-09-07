namespace CustomsDED.DTOs.PersonDTOs
{
    using CustomsDED.Common.Enums;

    public class PersonAddDTO : PersonBaseDTO
    {
        // public string FirstName { get; set; } = null!;

        // public string? MiddleName { get; set; }

        // public string LastName { get; set; } = null!;


        //  public DateTime? DateOfBirth { get; set; }

        //  public DateTime? DateOfInspection { get; set; }

        public string? Nationality { get; set; }

        public string? PersonalNumber { get; set; }

        public string? DocumentNumber { get; set; }

        public string? DocumentType { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string? IssuingCountry { get; set; }

        public SexType? SexType { get; set; }

        public string? AdditionInfo { get; set; }

    }
}
