using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class EditPaymentMethodsPage : ContentPage
{
	public EditPaymentMethodsPage(EditPaymentMethodsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}