using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    public class ClientReminderViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        private ClientProfile _clientProfile;
        public ICommand LoadCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public ClientReminderViewModel(ApiService apiService, ISessionService sessionService)
        {
            _ApiService = apiService;
            _sessionService = sessionService;
            SaveCommand = new Command(async () => await SaveRemider());
            BackCommand = new Command(async () => await GoBack());
            LoadClientProfile();
        }
        public int ClientId { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string? Notes { get; set; }
        public async Task SaveRemider()
        {
            
        }
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

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
    }
}
