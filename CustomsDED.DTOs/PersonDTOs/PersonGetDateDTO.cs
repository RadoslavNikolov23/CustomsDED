namespace CustomsDED.DTOs.PersonDTOs
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class PersonGetDateDTO : PersonBaseDTO
    {
        [ObservableProperty]
        public DateTime? dateOfInspection;

    }
}
