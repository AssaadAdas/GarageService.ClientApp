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
    [QueryProperty(nameof(VehileId), "vehileid")]
    public class EditVehicleOdometerViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        private ClientProfile _clientProfile;
        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set
            {
                if (_vehicle != value)
                {
                    _vehicle = value;
                    OnPropertyChanged(nameof(Vehicle));
                }
            }
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
        public ICommand BackCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand LoadClientCommand { get; }
        public ICommand SaveCommand { get; }
        private int _vehileid;
        public int VehileId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
                LoadCommand.Execute(null);
            }
        }

       
       

        public EditVehicleOdometerViewModel(ApiService apiService, ISessionService sessionService)
        {
            _ApiService = apiService;
            _sessionService = sessionService;
            SaveCommand = new Command(async () => await SaveVehile());
            BackCommand = new Command(async () => await GoBack());
            LoadClientCommand = new Command(async () => await LoadClientProfile());
            LoadClientCommand.Execute(null); 
            LoadCommand = new Command(async () => await LoadVehicle());
            LoadCommand.Execute(null);
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }


        public int VehicleTypeId { get; set; }

        public int ClientId { get; set; }

        public int Odometer { get; set; }
        
        private async Task LoadVehicle()
        {
            // Get current user ID from your authentication system
            int ClientId = GetCurrentUserId();

            var response = await _ApiService.GetVehicleByID(VehileId);
            if (response.IsSuccess)
            {
                Vehicle = response.Data;
                if (Vehicle != null)
                {
                    Odometer = Vehicle.Odometer;
                    OnPropertyChanged(nameof(Odometer));
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

        public async Task SaveVehile()
        {
            Vehicle.Odometer = Odometer;
            bool success = await _ApiService.UpdateVehicleAsync(Vehicle.Id, Vehicle);
            if (success)
            {
                await _ApiService.GetVehicleServicesHistory(Vehicle.Id, Odometer);
                await Shell.Current.DisplayAlert("Success", "Vehicle updated successfully", "OK");
                // Optionally, navigate back or clear the form
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update Vehicle", "OK");
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
