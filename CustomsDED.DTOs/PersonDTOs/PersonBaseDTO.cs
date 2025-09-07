namespace CustomsDED.DTOs.PersonDTOs
{
    public class PersonBaseDTO
    {
        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

    }
}
