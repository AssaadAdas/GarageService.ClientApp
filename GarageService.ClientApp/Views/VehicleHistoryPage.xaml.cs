using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class VehicleHistoryPage : ContentPage
{
	public VehicleHistoryPage(VehicleHistoryViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}