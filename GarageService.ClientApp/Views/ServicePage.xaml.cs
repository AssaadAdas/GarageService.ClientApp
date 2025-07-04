using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class ServicePage : ContentPage
{
	public ServicePage(VehiclesServiceViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}