namespace CustomsDED.Views;

using CustomsDED.ViewModels;

public partial class VehicleSearchPage : ContentPage
{
	
    public VehicleSearchPage(VehicleSearchViewModel vehicleSearchViewModel)
	{
		InitializeComponent();
		this.BindingContext = vehicleSearchViewModel;
    }
}