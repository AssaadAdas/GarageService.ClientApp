using CommunityToolkit.Maui;
using GarageService.ClientApp.Services;
using GarageService.ClientApp.ViewModels;
using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using Microsoft.Extensions.Logging;

namespace GarageService.ClientApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialDesignIcons");
            });
        // Register ViewModels

        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<BaseViewModel>();
        builder.Services.AddTransient<ClientRegistrationViewModel>();
        builder.Services.AddTransient<ClientDashboardViewModel>();
        builder.Services.AddTransient<EditClientProfileViewModel>();
        builder.Services.AddTransient<AddVehicleViewModel>();
        builder.Services.AddTransient<EditVehicleViewModel>();
        builder.Services.AddTransient<ReadNotificationViewModel>();
        builder.Services.AddTransient<VehiclesServiceViewModel>();
        builder.Services.AddTransient<VehiclesServiceTypeViewModel>();
        builder.Services.AddTransient<ServiceTypeViewModel>();
        builder.Services.AddTransient<VehiclesRefuelViewModel>();
        builder.Services.AddTransient<VehicleHistoryViewModel>();
        builder.Services.AddTransient<VehicleAppointmentViewModel>();
        builder.Services.AddTransient<ServicesTypeSetUpViewModel>();
        builder.Services.AddTransient<EditVehicleOdometerViewModel>();


        // Register Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ClientRegistrationPage>();
        builder.Services.AddTransient<ClientDashboardPage>();
        builder.Services.AddTransient<EditClientProfilePage>();
        builder.Services.AddTransient<AddVehiclePage>();
        builder.Services.AddTransient<EditVehiclePage>();
        builder.Services.AddTransient<NotificationDetailPage>();
        builder.Services.AddTransient<VehiclesService>();
        builder.Services.AddTransient<PremuimPage>();
        builder.Services.AddTransient<AddServiceTypePage>();
        builder.Services.AddTransient<VehiclesRefuelPage>();
        builder.Services.AddTransient<VehicleHistoryPage>();
        builder.Services.AddTransient<VehicleAppointmentPage>();
        builder.Services.AddTransient<ServicesTypeSetUpPage>();
        builder.Services.AddTransient<EditVehicleOdometerPage>();

        // Services
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<ServiceFormState>();
        //Models
        builder.Services.AddTransient<ServicePage>();



#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
