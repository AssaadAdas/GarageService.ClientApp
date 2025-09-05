
using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class VehicleAppointmentPage : ContentPage
{
	public VehicleAppointmentPage(VehicleAppointmentViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}