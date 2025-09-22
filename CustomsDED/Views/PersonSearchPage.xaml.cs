namespace CustomsDED.Views;

using CustomsDED.ViewModels;

public partial class PersonSearchPage : ContentPage
{
	public PersonSearchPage(PersonSearchViewModel personSearchViewModel)
	{
		InitializeComponent();
        this.BindingContext = personSearchViewModel;
    }
}