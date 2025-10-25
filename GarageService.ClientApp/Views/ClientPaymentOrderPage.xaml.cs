using GarageService.ClientApp.ViewModels;
using GarageService.ClientLib.Models;

namespace GarageService.ClientApp.Views;

public partial class ClientPaymentOrderPage : ContentPage
{
	public ClientPaymentOrderPage(ClientPaymentOrderViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    private void OnOfferCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.BindingContext is ClientPaymentMethod offer && e.Value)
        {
            if (BindingContext is ClientPaymentOrderViewModel vm)
                vm.SelectedPaymentMethod = offer;
        }
    }
}