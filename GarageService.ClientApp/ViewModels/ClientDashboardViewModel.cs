using GarageService.ClientApp.Services;
using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    public class ClientDashboardViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        private ClientProfile _clientProfile;
        private ObservableCollection<Vehicle> _Vehicles;
        
        public ClientProfile ClientProfile
        {
            get => _clientProfile;
            set
            {
                if (_clientProfile != value)
                {
                    _clientProfile = value;
                    OnPropertyChanged(nameof(ClientProfile));
                }
            }
        }
        private List<ClientPaymentOrder> _PendingOrders;
        public List<ClientPaymentOrder> PendingOrders
        {
            get => _PendingOrders;
            set
            {
                if (_PendingOrders != value)
                {
                    _PendingOrders = value;
                    OnPropertyChanged(nameof(PendingOrders));
                }
            }
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _Vehicles;
            set => SetProperty(ref _Vehicles, value);
        }

        private ObservableCollection<ClientNotification> _ClientNotifications;
        public ObservableCollection<ClientNotification> ClientNotifications
        {
            get => _ClientNotifications;
            set => SetProperty(ref _ClientNotifications, value);
        }

        private ObservableCollection<ClientNotification> _UnreadClientNotifications;
        public ObservableCollection<ClientNotification> UnreadClientNotifications
        {
            get => _UnreadClientNotifications;
            set => SetProperty(ref _UnreadClientNotifications, value);
        }

        public ICommand OpenHistoryCommand { get; }
        public ICommand PremuimCommand { get; }
        public ICommand AddVehicleCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand EditVehicleCommand { get; }
        public ICommand ShowPopUpCommand { get; }
        public ICommand AddServicesCommand { get; }
        public ICommand AddAppointmentCommand { get; }
        public ICommand EditProfileCommand { get; }
        public ICommand GoPendingOrderCommand { get; }
        public ICommand GoSettingCommand { get; }
        public ICommand ReadNoteCommand { get; }
        private readonly INavigationService _navigationService;

        private ClientPremiumRegistration _ClientPremiumRegistration;
        public ClientPremiumRegistration ClientPremiumRegistration
        {
            get => _ClientPremiumRegistration;
            set
            {
                if (_ClientPremiumRegistration != value)
                {
                    _ClientPremiumRegistration = value;
                    OnPropertyChanged(nameof(ClientPremiumRegistration));
                }
            }
        }

        public ClientDashboardViewModel(ApiService apiservice, ISessionService sessionService, INavigationService navigationService)
        {
            // Initialize properties and commands
            _ApiService = apiservice;
            _sessionService = sessionService;
            _navigationService = navigationService;
            OpenHistoryCommand = new Command(OpenHistory);
            AddVehicleCommand = new Command(async () => await AddVehicle());
            PremuimCommand = new Command(async () => await LoadPremuim());
            LogOutCommand = new Command(async () => await LogOut());
            EditVehicleCommand = new Command<Vehicle>(async (vehicle) => await EditVehicle(vehicle));
            ShowPopUpCommand = new Command<Vehicle>(async (vehicle) => await ShowMenu(vehicle));
            AddServicesCommand = new Command<Vehicle>(async (vehicle) => await AddServices(vehicle));
            AddAppointmentCommand = new Command(AddAppointment);
            EditProfileCommand = new Command(async () => await EditProfile());
            GoPendingOrderCommand = new Command<ClientPaymentOrder>(async (ClientPaymentOrder) => await GoPendingOrder(ClientPaymentOrder));
            GoSettingCommand = new Command(async () => await GoSettingMethods());
            ReadNoteCommand = new Command<ClientNotification>(async (clientnotification) => await ReadNote(clientnotification));
            // Load data here
            LoadClientProfile();
            
        }
        private async Task GoPendingOrder(ClientPaymentOrder PendingOrder)
        {
            await Shell.Current.GoToAsync($"{nameof(PaymentPage)}?PaymentOrderid={PendingOrder.Id}");
        }
        private async Task GoSettingMethods()
        {
            int ClientId = GetCurrentUserId();
            var popup = new SettingsMenuPopup();
            await _navigationService.ShowPopupAsync(popup);
            
        }
        public async Task LoadClientPremuim(int ClientID)
        {
            string ErrorMessage = string.Empty;
            ClientPremiumRegistration = await _ApiService.GetActiveRegistrationByClientId(ClientID);
            if (ClientPremiumRegistration == null)
            {
                ErrorMessage = "No active registration found for this Client.";
            }
        }
        private void OpenHistory() { /* Navigate to history page */ }
        private async Task AddVehicle()
        { 
            await Shell.Current.GoToAsync($"{nameof(AddVehiclePage)}");
        }
        private async Task LogOut()
        { 
        }
        private async Task LoadPremuim()
        {
            await Shell.Current.GoToAsync($"{nameof(PremuimPage)}");
        }
        private async Task EditVehicle(Vehicle vehicle)
        {
            await Shell.Current.GoToAsync($"{nameof(EditVehiclePage)}?vehileid={vehicle.Id}");
        }
        
       

        private async Task AddServices(Vehicle vehicle)
        {
            await Shell.Current.GoToAsync($"{nameof(ServicePage)}?VehicleId={vehicle.Id}");
        }
        private async Task ShowMenu(Vehicle vehicle)
        {
            var popup = new PopupMenuPage(vehicle.Id);
            await _navigationService.ShowPopupAsync(popup);
        }
       
        private void AddAppointment() { /* Open add appointment dialog */ }
        private async Task EditProfile()
        {
            await Shell.Current.GoToAsync($"{nameof(EditClientProfilePage)}");
        }
        private async Task ReadNote(ClientNotification notification)
        {
            await Shell.Current.GoToAsync($"{nameof(NotificationDetailPage)}?noteId={notification.Id}");
        }
        public async Task LoadClientProfile()
        {
            // Get current user ID from your authentication system
            int ClientId = GetCurrentUserId();

            var response = await _ApiService.GetClientByID(ClientId);
            if (response.IsSuccess)
            {
                ClientProfile = response.Data;
                LoadClientPremuim(ClientProfile.Id);
                LoadPendingOrders(ClientProfile.Id);
            }
        }
        public async Task LoadPendingOrders(int ClientID)
        {
            string ErrorMessage = string.Empty;
            var response = await _ApiService.GetPendingPaymentOrderByID(ClientID);
            if (response != null && response.Data != null)
            {
                PendingOrders = new List<ClientPaymentOrder>(response.Data);
            }
        }
        private int GetCurrentUserId()
        {
            // Implement your actual user ID retrieval logic
            int userId;
            string userType;
            int profileId = 1;
            if (_sessionService.IsLoggedIn)
            {
                userId = _sessionService.UserId;
                userType = _sessionService.UserType.ToString();
                profileId = _sessionService.ProfileId;
            }
            else
            {
                // If not logged in, you might want to redirect to login page or throw an exception
                throw new UnauthorizedAccessException("User is not logged in.");
            }

           return profileId; // Placeholder
        }
    }
}