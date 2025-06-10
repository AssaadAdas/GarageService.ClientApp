using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class ClientRegistrationPage : ContentPage
{
    public ClientRegistrationPage(ClientRegistrationViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}