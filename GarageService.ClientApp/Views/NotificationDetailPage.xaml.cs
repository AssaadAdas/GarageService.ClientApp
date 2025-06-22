using GarageService.ClientApp.ViewModels;

namespace GarageService.ClientApp.Views;

public partial class NotificationDetailPage : ContentPage
{
	public NotificationDetailPage(ReadNotificationViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}