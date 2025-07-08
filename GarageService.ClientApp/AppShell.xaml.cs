using GarageService.ClientApp.Views;

namespace GarageService.ClientApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("Main", typeof(MainPage));
            Routing.RegisterRoute("ClientRegistration", typeof(ClientRegistrationPage));
            Routing.RegisterRoute("ClientDashboard", typeof(ClientDashboardPage));
            Routing.RegisterRoute(nameof(EditClientProfilePage), typeof(EditClientProfilePage));
            Routing.RegisterRoute(nameof(NotificationDetailPage), typeof(NotificationDetailPage));
            Routing.RegisterRoute(nameof(AddVehiclePage), typeof(AddVehiclePage));
            Routing.RegisterRoute(nameof(EditVehiclePage), typeof(EditVehiclePage));
            Routing.RegisterRoute(nameof(PremuimPage), typeof(PremuimPage));
            Routing.RegisterRoute(nameof(ServicePage), typeof(ServicePage));
            Routing.RegisterRoute(nameof(AddServiceTypePage), typeof(AddServiceTypePage));


            // Set initial route
            //CurrentItem = Items[0];

        }
       
    }
}
