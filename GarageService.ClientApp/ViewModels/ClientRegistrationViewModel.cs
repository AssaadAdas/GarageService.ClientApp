using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
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
    public class ClientRegistrationViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int UserTypeid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private int _selectedCountryId;
        public int CountryId {
            get => _selectedCountryId;
            set
            {
                SetProperty(ref _selectedCountryId, value);
                // If you need to find the full country object:
                SelectedCountry = Countries?.FirstOrDefault(c => c.Id == value);
            }
        }
        private string _phoneExt;
        public string PhoneExt
        {
            get => _phoneExt;
            set => SetProperty(ref _phoneExt, value);
        }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; } = 2;
        public bool IsPremium { get; set; } = false;

        private List<Country> _countries;
        private Country _selectedCountry;
        public List<Country> Countries
        {
            get => _countries;
            set => SetProperty(ref _countries, value);
        }

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (SetProperty(ref _selectedCountry, value))
                {
                    CountryId = value?.Id ?? 0;
                    PhoneExt = value?.PhoneExt ?? string.Empty; // Set PhoneExt from selected country
                }
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }
        public ClientRegistrationViewModel(ApiService apiservice)
        {
            _ApiService = apiservice;
            RegisterCommand = new Command(async () => await Register());
            BackCommand = new Command(async () => await GoBack());
            LoadCountries();
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }
        private async void LoadCountries()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetCountriesAsync();

                if (apiResponse.IsSuccess)
                {
                    Countries = apiResponse.Data;
                }
                else
                {
                    ErrorMessage = apiResponse.ErrorMessage;
                    // Optionally log the error
                    Debug.WriteLine($"API Error: {apiResponse.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred";
                Debug.WriteLine($"Exception: {ex}");
            }

        }
        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword) != string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }
            var userTypeResponse = await _ApiService.GetUserType(2); // 2 = client user type
            if (!userTypeResponse.IsSuccess || userTypeResponse.Data == null)
            {
                await Shell.Current.DisplayAlert("Error", userTypeResponse.ErrorMessage ?? "Failed to get user type", "OK");
                return;
            }
            var usertype = userTypeResponse.Data;
            // Create user
            var user = new User
            {
                Username = Username,
                Password = Password, // Hash this in production
                UserTypeid = usertype.Id
            };

            var (isSuccess, message, registeredUser) = await _ApiService.RegisterUserAsync(user);
            if (!isSuccess)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create user", "OK");
                return;
            }

            // Get the user with ID

            
            var userAdded = await _ApiService.GetUserByUsername(Username);
            if (userAdded != null && userAdded.IsSuccess)
            {
                user  = userAdded.Data; // Extract the User object
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Not Found", "OK");
                return;
            }


            // Create client profile
            var clientProfile = new ClientProfile
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    CountryId = CountryId,
                    PhoneExt = PhoneExt,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    Address = Address,
                    IsPremium = false,
                    UserId = user.Id
                };

            var profileAddedResponse = await _ApiService.ClientRegister(clientProfile);

            if (profileAddedResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "Registration successful", "OK");
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create client profile", "OK");
            }
        }
    }
}
