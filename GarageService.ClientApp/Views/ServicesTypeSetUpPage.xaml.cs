using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class ServicesTypeSetUpPage : ContentPage
{
	public ServicesTypeSetUpPage(ServicesTypeSetUpViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}