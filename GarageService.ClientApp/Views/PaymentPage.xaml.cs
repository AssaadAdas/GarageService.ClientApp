
using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class PaymentPage : ContentPage
{
	public PaymentPage(PaymentViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}