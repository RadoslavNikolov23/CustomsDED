namespace CustomsDED.Views;

using CustomsDED.ViewModels;

public partial class MrzPersonPage : ContentPage
{
	public MrzPersonPage(MrzPersonViewModel mrzPersonViewModel)
	{
		InitializeComponent();
        this.BindingContext = mrzPersonViewModel;

    }
}