namespace GarageService.ClientApp.Views;

public partial class ClientPaymentOrderPage : ContentPage
{
	public ClientPaymentOrderPage()
	{
		InitializeComponent();
	}
    private void OnOfferCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //if (sender is RadioButton radioButton && radioButton.BindingContext is GaragePaymentMethod offer && e.Value)
        //{
        //    if (BindingContext is GaragePaymentOrdersViewModel vm)
        //        vm.SelectedPaymentMethod = offer;
        //}
    }
}