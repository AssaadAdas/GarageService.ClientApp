using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class AddVehiclePage : ContentPage
{
	public AddVehiclePage(AddVehicleViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}