using CommunityToolkit.Maui;
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

        // Services
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddSingleton<ApiService>();

        //Models
        builder.Services.AddTransient<ServicePage>();



#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
