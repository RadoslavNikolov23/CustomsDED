namespace CustomsDED.ViewModels
{
    using CommunityToolkit.Mvvm.Input;

    public partial class VehicleSearchViewModel : BaseViewModel
    {
        public VehicleSearchViewModel()
        {
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
    
}
