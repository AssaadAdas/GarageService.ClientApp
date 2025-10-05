using CommunityToolkit.Maui.Views;
using GarageService.ClientApp.ViewModels;
using System.Windows.Input;

namespace GarageService.ClientApp.Views;

public partial class PopupMenuPage : Popup
{
    public ICommand AddServicesCommand { get; }
    private int _vehicleid;
    public PopupMenuPage(int VehicleId)//int VehileId)
    {
        InitializeComponent();
        _vehicleid = VehicleId;
        this.BindingContext = this; // Important: Set BindingContext to self
    }

    private void EditVehicleClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(EditVehiclePage)}?vehileid={_vehicleid}");
        this.CloseAsync();
    }
    private void EditVehicleOdoClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(EditVehicleOdometerPage)}?vehileid={_vehicleid}");
        this.CloseAsync();
    }
    private void AddServicesClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(ServicePage)}?vehileid={_vehicleid}");
        this.CloseAsync();
    }

    private void RefuelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(VehiclesRefuelPage)}?vehileid={_vehicleid}");
        this.CloseAsync();
    }

    private void HistoryClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(VehicleHistoryPage)}?vehicleid={_vehicleid}");
        this.CloseAsync();
    }
    private void SetUpServiceTypesClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(ServicesTypeSetUpPage)}?vehileid={_vehicleid}");
        this.CloseAsync();
    }

    private void AppointmentsClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"{nameof(VehicleAppointmentPage)}?vehicleid={_vehicleid}");
        this.CloseAsync();
    }
}