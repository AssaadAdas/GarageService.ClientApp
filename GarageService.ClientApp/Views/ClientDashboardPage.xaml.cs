using GarageService.ClientApp.ViewModels;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;

namespace GarageService.ClientApp.Views;

public partial class ClientDashboardPage : ContentPage
{
    public ClientDashboardPage(ClientDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnPlusClicked(object sender, EventArgs e)
    {
        //FlyoutMenu.IsPresented = true;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ClientDashboardViewModel vm)
        {
            await vm.LoadClientProfile(); // Make sure this method fetches the latest profile
        }
    }
}
