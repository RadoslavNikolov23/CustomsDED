namespace CustomsDED.ViewModels
{
    using CommunityToolkit.Mvvm.Input;

    public partial class PersonSearchViewModel : BaseViewModel
    {
        public PersonSearchViewModel()
        {
            
        }

        [RelayCommand]
        private async Task ReportAProblem()
        {
            await SendEmailWithReport();
        }
    }
}
