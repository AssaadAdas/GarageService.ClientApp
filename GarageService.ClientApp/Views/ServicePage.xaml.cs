using GarageService.ClientApp.ViewModels;
using Microsoft.Maui.Layouts;

namespace GarageService.ClientApp.Views;

public partial class ServicePage : ContentPage
{
	public ServicePage(VehiclesServiceViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}