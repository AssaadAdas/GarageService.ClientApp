
using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class AddServiceTypePage : ContentPage
{
	public AddServiceTypePage(VehiclesServiceTypeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}