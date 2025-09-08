namespace CustomsDED.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class LicensePlateViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isCameraViewVisable = false;
        
        [ObservableProperty]
        private bool isPlateFrameVisable = false;

        [ObservableProperty]
        private bool isVisiableTakePlatePictureButton = false;

        [ObservableProperty]
        private bool isVisiableGoBackTakePlateButton = false;
        
        [ObservableProperty]
        private string plateLicenseEntry = "";
        
        [ObservableProperty]
        private string additionInfoEntry = "";

        public LicensePlateViewModel()
        {
            
        }

        [RelayCommand]
        private async Task TakePlatePicture()
        {

        }

        [RelayCommand]
        private async Task GoBackTakePlate()
        {

        }

        [RelayCommand]
        private async Task OpenPlateCamera()
        {

        }

        [RelayCommand]
        private async Task SavePlate()
        {

        }

        [RelayCommand]
        private async Task ReportAProblem()
        {

        }
    }
}
