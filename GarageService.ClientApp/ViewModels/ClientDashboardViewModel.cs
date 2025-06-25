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
        private ObservableCollection<ClientNotification> _ClientNotifications;
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

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _Vehicles;
            set => SetProperty(ref _Vehicles, value);
        }

        public ObservableCollection<ClientNotification> ClientNotifications
        {
            get => _ClientNotifications;
            set => SetProperty(ref _ClientNotifications, value);
        }
        public ICommand OpenHistoryCommand { get; }
        public ICommand AddVehicleCommand { get; }
        public ICommand EditVehicleCommand { get; }
        public ICommand AddAppointmentCommand { get; }
        public ICommand EditProfileCommand { get; }

        public ICommand ReadNoteCommand { get; }

        public ClientDashboardViewModel(ApiService apiservice, ISessionService sessionService)
        {
            // Initialize properties and commands
            _ApiService = apiservice;
            _sessionService = sessionService;

            OpenHistoryCommand = new Command(OpenHistory);
            AddVehicleCommand = new Command(async () => await AddVehicle());
            EditVehicleCommand = new Command(async () => await EditVehicle());
            AddAppointmentCommand = new Command(AddAppointment);
            EditProfileCommand = new Command(async () => await EditProfile());
            ReadNoteCommand = new Command<ClientNotification>(async (clientnotification) => await ReadNote(clientnotification));
            // Load data here
            LoadClientProfile();
        }


        private void OpenHistory() { /* Navigate to history page */ }
        private async Task AddVehicle()
        { 
            await Shell.Current.GoToAsync($"{nameof(AddVehiclePage)}"); 
        }
        private async Task EditVehicle()
        {
            await Shell.Current.GoToAsync($"{nameof(EditVehiclePage)}");
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