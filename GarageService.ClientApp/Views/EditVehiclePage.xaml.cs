using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class EditVehiclePage : ContentPage
{
	public EditVehiclePage(EditVehicleViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}