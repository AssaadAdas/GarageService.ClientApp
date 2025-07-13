using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace GarageService.ClientApp.Views;

public partial class PopupMenuPage : Popup
{
    public ICommand AddServicesCommand { get; }
    private int _vehileid;
    public PopupMenuPage(int VehileId)
    {
        InitializeComponent();
        _vehileid = VehileId;
        this.BindingContext = this; // Important: Set BindingContext to self
    }

    private void  AddServicesClicked(object sender, EventArgs e)
    {
         Shell.Current.GoToAsync($"{nameof(ServicePage)}?vehileid={_vehileid}");
         this.CloseAsync(); // Close the popup after navigation
    }

    private void RefuelClicked(object sender, EventArgs e)
    {

    }

    private void HistoryClicked(object sender, EventArgs e)
    {

    }
    private void SetUpServiceTypesClicked(object sender, EventArgs e)
    {

    }

    private void AppointmentsClicked(object sender, EventArgs e)
    {

    }
}