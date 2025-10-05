using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class EditVehicleOdometerPage : ContentPage
{
	public EditVehicleOdometerPage(EditVehicleOdometerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}