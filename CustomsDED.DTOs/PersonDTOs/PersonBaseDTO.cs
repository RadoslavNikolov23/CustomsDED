namespace CustomsDED.DTOs.PersonDTOs
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class PersonBaseDTO : ObservableObject
    {
        [ObservableProperty]
        public string? firstName;

        [ObservableProperty]
        public string? middleName;

        [ObservableProperty]
        public string? lastName;

        [ObservableProperty]
        public DateTime? dateOfBirth;

    }
}
