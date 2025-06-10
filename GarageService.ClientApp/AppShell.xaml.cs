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

            // Set initial route
            //CurrentItem = Items[0];

        }
       
    }
}
