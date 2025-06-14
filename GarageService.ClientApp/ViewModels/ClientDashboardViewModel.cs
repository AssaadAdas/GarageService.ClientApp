using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        //public ObservableCollection<Vehicle> Vehicles { get; set; }
        //public ObservableCollection<VehicleAppointment> VehicleAppointments { get; set; }

        public ICommand OpenHistoryCommand { get; }
        public ICommand AddVehicleCommand { get; }
        public ICommand AddAppointmentCommand { get; }

        public ClientDashboardViewModel(ApiService apiservice, ISessionService sessionService)
        {
            // Initialize properties and commands
            _ApiService = apiservice;
            _sessionService = sessionService;

            OpenHistoryCommand = new Command(OpenHistory);
            AddVehicleCommand = new Command(AddVehicle);
            AddAppointmentCommand = new Command(AddAppointment);

            // Load data here
            LoadClientProfile();
        }

        private void OpenHistory() { /* Navigate to history page */ }
        private void AddVehicle() { /* Open add vehicle dialog */ }
        private void AddAppointment() { /* Open add appointment dialog */ }
        private async void LoadClientProfile()
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