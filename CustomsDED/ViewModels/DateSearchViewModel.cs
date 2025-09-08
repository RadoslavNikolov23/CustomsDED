namespace CustomsDED.ViewModels
{
    using CommunityToolkit.Mvvm.Input;

    public partial class DateSearchViewModel : BaseViewModel
    {
        public DateSearchViewModel()
        {
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
