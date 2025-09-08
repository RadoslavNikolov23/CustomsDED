namespace CustomsDED.Views;

using CustomsDED.ViewModels;

public partial class DateSearchPage : ContentPage
{
	public DateSearchPage(DateSearchViewModel dateSearchViewModel)
	{
		InitializeComponent();
        this.BindingContext = dateSearchViewModel;

    }
}