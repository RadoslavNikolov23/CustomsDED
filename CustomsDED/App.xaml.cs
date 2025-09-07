namespace CustomsDED
{
    using Plugin.Maui.OCR;

    using System.Globalization;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.RegisterSingleton(OcrPlugin.Default);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("bg-BG");

            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}