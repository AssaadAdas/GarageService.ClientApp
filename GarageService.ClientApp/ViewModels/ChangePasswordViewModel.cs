using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System.Windows.Input;


using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace GarageService.ClientApp.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private readonly ISessionService _sessionService;
        private readonly ApiService _ApiService;
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand BackCommand { get; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string ConfirmPassword { get; set; }
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

        public ChangePasswordViewModel(ISessionService sessionService, ApiService apiservice)
        {
            _sessionService = sessionService;
            _ApiService = apiservice;
            SaveCommand = new Command(async () => await ChangePasswordAsync());
            LoadCommand = new Command(async () => await LoadProfile());
            BackCommand = new Command(async () => await GoBack());
            LoadCommand.Execute(null);
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }
        private async Task ChangePasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }

            if (ConfirmPassword != Password)
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            // Use existing PasswordChangeRequest type from the models
            var changePassword = new PasswordChangeRequest
            {
                OldPassword = OldPassword,
                NewPassword = Password
            };

            // Use ClientProfile.UserId (ensure ClientProfile is loaded)
            var userId = ClientProfile?.UserId ?? GetCurrentUserId();

            bool success = await _ApiService.ChangePasswordAsync(userId, changePassword);

            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Password changed successfully", "OK");
                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to change password", "OK");
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

        private async Task LoadProfile()
        {
            int ClientId = GetCurrentUserId();

            var response = await _ApiService.GetClientByID(ClientId);
            if (response.IsSuccess)
            {
                ClientProfile = response.Data;
            }
        }
    }
}