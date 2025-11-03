using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class PaymentMethodsPage : ContentPage
{
	public PaymentMethodsPage(PaymentMethodsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}