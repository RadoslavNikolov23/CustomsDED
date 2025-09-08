namespace CustomsDED
{
    using CommunityToolkit.Maui.Alerts;
    using CommunityToolkit.Maui.Core;

    using CustomsDED.Resources.Localization;

    public partial class MainPage : Shell
    {
        private DateTime lastBackPressed;
        private readonly TimeSpan backPressThreshold = TimeSpan.FromSeconds(2);

        public MainPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            var now = DateTime.Now;

            if (now - this.lastBackPressed <= this.backPressThreshold)
            {
                Application.Current!.Quit();
            }
            else
            {
                this.lastBackPressed = now;
                ShowToast(AppResources.PressBackAgainToExit);
            }

            return true;
        }

        private async void ShowToast(string message)
        {
            await Toast.Make(message, ToastDuration.Short).Show();
        }
    }
}
