namespace CustomsDED.ViewModels
{
    using CommunityToolkit.Mvvm.Input;

    public partial class MrzPersonViewModel : BaseViewModel
    {
        public MrzPersonViewModel()
        {
            
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
