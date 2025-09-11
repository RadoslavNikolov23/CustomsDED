namespace CustomsDED.DTOs.PersonDTOs
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CustomsDED.Common.Enums;

    public partial class PersonAddDTO : PersonBaseDTO
    {
        // public string FirstName { get; set; } = null!;

        // public string? MiddleName { get; set; }

        // public string LastName { get; set; } = null!;


        //  public DateTime? DateOfBirth { get; set; }

        //  public DateTime? DateOfInspection { get; set; }

        [ObservableProperty]
        public string? nationality;

        [ObservableProperty]
        public string? personalId;

        [ObservableProperty]
        public string? documentNumber;

        [ObservableProperty]
        public string? documentType;

        [ObservableProperty]
        public DateTime? expirationDate;

        [ObservableProperty]
        public string? issuingCountry;

        [ObservableProperty]
        public SexType? sexType;

        [ObservableProperty]
        public string? additionInfo;

    }
}
