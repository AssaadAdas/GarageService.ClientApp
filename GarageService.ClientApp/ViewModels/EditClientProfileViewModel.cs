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

    public class EditClientProfileViewModel : BaseViewModel
    {
        private readonly ISessionService _sessionService;
        private readonly ApiService _ApiService;
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
       
        public ICommand SaveCommand { get; }

        public ICommand LoadCommand { get; }
        public ICommand LoadCountriesCommand { get; }
        public ICommand BackCommand { get; }

        // client proporties
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private int _selectedCountryId;
        public int CountryId
        {
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

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
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

        public EditClientProfileViewModel(ISessionService sessionService,ApiService apiservice)
        {
            _sessionService = sessionService;
            _ApiService = apiservice;

            SaveCommand = new Command(async () => await SaveProfile());
            
            BackCommand = new Command(async () => await GoBack());
            LoadCountriesCommand = new Command(async () => await LoadCountries());
            
            LoadCountriesCommand.Execute(null);
            // Load profile when ViewModel is created
            LoadCommand = new Command(async () => await LoadProfile());
            LoadCommand.Execute(null);
            
        }

        private async Task LoadCountries()
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

       
        private async Task SaveProfile()
        {
            try
            {
                ClientProfile.FirstName = FirstName;
                ClientProfile.LastName = LastName;
                ClientProfile.Email = Email;
                ClientProfile.PhoneExt = PhoneExt;
                ClientProfile.PhoneNumber = PhoneNumber;
                ClientProfile.CountryId = CountryId;

                ClientProfile.Address = Address;
                bool success = await _ApiService.UpdateClientProfileAsync(ClientProfile.Id, ClientProfile);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Success", "Profile updated successfully", "OK");
                    await Shell.Current.GoToAsync(".."); // This pops the Edit page and returns to the dashboard
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update profile", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
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
            // Get current user ID from your authentication system
            int ClientId = GetCurrentUserId();

            var response = await _ApiService.GetClientByID(ClientId);
            if (response.IsSuccess)
            {
                ClientProfile = response.Data;
                if (ClientProfile !=null)
                {
                    FirstName = ClientProfile.FirstName;
                    LastName = ClientProfile.LastName;
                    Email = ClientProfile.Email;
                    Address = ClientProfile.Address;
                    PhoneNumber = ClientProfile.PhoneNumber;
                    CountryId = ClientProfile.CountryId;
                    PhoneExt = ClientProfile.PhoneExt;
                    // Set the selected country based on the CountryId
                    SelectedCountry = Countries?.FirstOrDefault(c => c.Id == CountryId);

                    OnPropertyChanged(nameof(FirstName));
                    OnPropertyChanged(nameof(LastName));
                    OnPropertyChanged(nameof(Email));
                    OnPropertyChanged(nameof(PhoneNumber));
                    OnPropertyChanged(nameof(Address));
                    OnPropertyChanged(nameof(CountryId));

                    OnPropertyChanged(nameof(PhoneExt));
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }
    }
}
