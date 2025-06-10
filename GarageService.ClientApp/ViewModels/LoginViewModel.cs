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
        //private readonly DatabaseService _databaseService;
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
                         ISecureStorage secureStorage)
        {
            //_databaseService = databaseService;
            _sessionService = sessionService;
            _secureStorage = secureStorage;

            LoginCommand = new Command(async () => await Login());
            RegisterCommand = new Command(async () => await Register());

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
                    //Username = savedUsername;
                    //Password = savedPassword;
                    //RememberMe = true;

                    //OnPropertyChanged(nameof(Username));
                    //OnPropertyChanged(nameof(Password));
                    //OnPropertyChanged(nameof(RememberMe));

                    // Auto-login
                    //await Login();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Auto-login failed: {ex.Message}");
            }
        }
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            //var user = await _databaseService.GetUserByUsername(Username, Password);

            //if (user == null) // In production, use password hashing
            //{
            //    await Shell.Current.DisplayAlert("Error", "Invalid username or password", "OK");
            //    return;
            //}

            //if (RememberMe)
            //{
            //    await _secureStorage.SetAsync("remember_username", Username);
            //    await _secureStorage.SetAsync("remember_password", Password);
            //}
            //else
            //{
            //    _secureStorage.Remove("remember_username");
            //    _secureStorage.Remove("remember_password");
            //}
            
            // var clientProfile = await _databaseService.GetClientProfileByUserId(user.Id);
            // _sessionService.CreateSession(user, clientProfile);
            

            // Navigate based on user type
           
            await Shell.Current.GoToAsync("//ClientDashboard");
            
        }

        private async Task Register()
        {
            await Shell.Current.GoToAsync("//ClientRegistration");
        }
    }

}
