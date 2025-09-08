namespace CustomsDED.DTOs.PersonDTOs
{
    public class PersonGetTextDTO : PersonBaseDTO
    {
        public string? PersonalId { get; set; }

        public DateTime? DateOfInspection { get; set; }
    }
}
