using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private bool _RememberMe;

        private readonly ISessionService _sessionService;
        private readonly ApiService _apiService;
        private readonly ISecureStorage _secureStorage;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        //public bool /*RememberMe*/ { get; set; }
        public bool RememberMe
        {
            get => _RememberMe;
            set => SetProperty(ref _RememberMe, value);
        }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel( ISessionService sessionService,
                         ISecureStorage secureStorage,
                         ApiService apiService)
        {
            //_databaseService = databaseService;
            _apiService = apiService;
            _sessionService = sessionService;
            _secureStorage = secureStorage;

            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await Register());
            _apiService = apiService;

            // Check for saved credentials on startup
            Task.Run(async () => await CheckAutoLogin());
        }

        private async Task CheckAutoLogin()
        {
            try
            {
                // Check if we have saved credentials
                var savedUsername = await _secureStorage.GetAsync("remember_username");
                var savedPassword = await _secureStorage.GetAsync("remember_password");

                if (!string.IsNullOrEmpty(savedUsername))
                {
                    Username = savedUsername;
                    Password = savedPassword;
                    RememberMe = true;

                    OnPropertyChanged(nameof(Username));
                    OnPropertyChanged(nameof(Password));
                    OnPropertyChanged(nameof(RememberMe));

                    // Auto-login
                    //await LoginAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Auto-login failed: {ex.Message}");
            }
        }
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            try
            {
                var response = await _apiService.LoginAsync(Username, Password);

                // Store the token securely

                if (RememberMe)
                {
                    await _secureStorage.SetAsync("remember_username", Username);
                    await _secureStorage.SetAsync("remember_password", Password);
                }
                else
                {
                    _secureStorage.Remove("remember_username");
                    _secureStorage.Remove("remember_password");
                }

                // Navigate to the main page
                var Clientresponse = await _apiService.GetClientByUserID(response.User.Id);
                _sessionService.CreateSession(response.User, Clientresponse.Data);
                await Shell.Current.GoToAsync("//ClientDashboard");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task Register()
        {
            await Shell.Current.GoToAsync("//ClientRegistration");
        }
    }

}
