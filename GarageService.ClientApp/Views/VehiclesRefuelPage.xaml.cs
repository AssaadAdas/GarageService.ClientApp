using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class VehiclesRefuelPage : ContentPage
{
	public VehiclesRefuelPage(VehiclesRefuelViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}