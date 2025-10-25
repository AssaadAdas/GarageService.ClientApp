using GarageService.ClientApp.ViewModels;
using GarageService.ClientLib.Models;

namespace GarageService.ClientApp.Views;

public partial class PremuimPage : ContentPage
{
	public PremuimPage(PremiumOffersViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    private void OnOfferCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.BindingContext is PremiumOffer offer && e.Value)
        {
            if (BindingContext is PremiumOffersViewModel vm)
                vm.SelectedPremiumOffer = offer;
        }
    }
}