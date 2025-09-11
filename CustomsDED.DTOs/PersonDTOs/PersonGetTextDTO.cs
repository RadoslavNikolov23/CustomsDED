namespace CustomsDED.DTOs.PersonDTOs
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class PersonGetTextDTO : PersonBaseDTO
    {
        [ObservableProperty]
        public string? personalId;

        [ObservableProperty]
        public DateTime? dateOfInspection;
    }
}
