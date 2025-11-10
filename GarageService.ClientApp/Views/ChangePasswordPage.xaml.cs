using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage(ChangePasswordViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}