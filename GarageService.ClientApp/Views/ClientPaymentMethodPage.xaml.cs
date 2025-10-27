using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class ClientPaymentMethodPage : ContentPage
{
	public ClientPaymentMethodPage(ClientPaymentMethodViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}