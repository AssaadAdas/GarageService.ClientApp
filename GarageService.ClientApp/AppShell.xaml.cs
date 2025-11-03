using GarageService.ClientApp.Views;

namespace GarageService.ClientApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute("Main", typeof(MainPage));
            Routing.RegisterRoute(nameof(ClientRegistrationPage), typeof(ClientRegistrationPage));
            Routing.RegisterRoute(nameof(ClientDashboardPage), typeof(ClientDashboardPage));
            Routing.RegisterRoute(nameof(EditClientProfilePage), typeof(EditClientProfilePage));
            Routing.RegisterRoute(nameof(NotificationDetailPage), typeof(NotificationDetailPage));
            Routing.RegisterRoute(nameof(AddVehiclePage), typeof(AddVehiclePage));
            Routing.RegisterRoute(nameof(EditVehiclePage), typeof(EditVehiclePage));
            Routing.RegisterRoute(nameof(PremuimPage), typeof(PremuimPage));
            Routing.RegisterRoute(nameof(ServicePage), typeof(ServicePage));
            Routing.RegisterRoute(nameof(VehiclesRefuelPage), typeof(VehiclesRefuelPage));
            Routing.RegisterRoute(nameof(AddServiceTypePage), typeof(AddServiceTypePage));
            Routing.RegisterRoute(nameof(VehicleHistoryPage), typeof(VehicleHistoryPage));
            Routing.RegisterRoute(nameof(VehicleAppointmentPage), typeof(VehicleAppointmentPage));
            Routing.RegisterRoute(nameof(ServicesTypeSetUpPage), typeof(ServicesTypeSetUpPage));
            Routing.RegisterRoute(nameof(EditVehicleOdometerPage), typeof(EditVehicleOdometerPage));
            Routing.RegisterRoute(nameof(ClientPaymentOrderPage), typeof(ClientPaymentOrderPage));
            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
            Routing.RegisterRoute(nameof(LastServicePage), typeof(LastServicePage));
            Routing.RegisterRoute(nameof(ClientPaymentMethodPage), typeof(ClientPaymentMethodPage));
            Routing.RegisterRoute(nameof(PaymentMethodsPage), typeof(PaymentMethodsPage));
            Routing.RegisterRoute(nameof(EditPaymentMethodsPage), typeof(EditPaymentMethodsPage));


            // Set initial route
            CurrentItem = Items[0];

        }
       
    }
}
