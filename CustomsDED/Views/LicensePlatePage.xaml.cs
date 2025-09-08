namespace CustomsDED.Views;

using CustomsDED.ViewModels;

public partial class LicensePlatePage : ContentPage
{
	public LicensePlatePage(LicensePlateViewModel licensePlateViewModel)
	{
		InitializeComponent();
        this.BindingContext = licensePlateViewModel;
    }
}