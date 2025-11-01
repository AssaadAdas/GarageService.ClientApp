
using GarageService.ClientApp.ViewModels;


namespace GarageService.ClientApp.Views;

public partial class LastServicePage : ContentPage
{
	public LastServicePage(LastServiceViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}