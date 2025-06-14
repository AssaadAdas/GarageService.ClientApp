using CommunityToolkit.Maui;
using GarageService.ClientApp.ViewModels;
using GarageService.ClientApp.Views;
using GarageService.ClientLib.Services;
using Microsoft.Extensions.Logging;

namespace GarageService.ClientApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
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

        // Register Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ClientRegistrationPage>();
        builder.Services.AddTransient<ClientDashboardPage>();
        builder.Services.AddTransient<EditClientProfilePage>();

        // Services
        builder.Services.AddSingleton<ISessionService, SessionService>();
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddSingleton<ApiService>();

        


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
