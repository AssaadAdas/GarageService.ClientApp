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
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
        // Register ViewModels
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<BaseViewModel>();
        builder.Services.AddTransient<ClientRegistrationViewModel>();

        // Register Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ClientRegistrationPage>();
        builder.Services.AddTransient<ClientDashboardPage>();

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
