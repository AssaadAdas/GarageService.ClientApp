using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class EditClientProfilePage : ContentPage
{
	public EditClientProfilePage(EditClientProfileViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}