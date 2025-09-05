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
    [QueryProperty(nameof(VehicleId), "vehicleid")]
    public class VehicleAppointmentViewModel : BaseViewModel
    {
        public VehicleAppointmentViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoadVehileCommand = new Command(async () => await LoadVehicle());
            LoadGarageCommand = new Command(async () => await LoadGarages());
            SaveCommand = new Command(async () => await SaveVehileAppointment());
            BackCommand = new Command(async () => await GoBack());
            LoadVehileCommand.Execute(null);
            LoadGarageCommand.Execute(null);
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        private readonly ApiService _apiService;
        public ICommand LoadVehileCommand { get; }
        public ICommand LoadGarageCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        private int _vehicleId;
        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged(nameof(VehicleAppointment));
            }
        }
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
        private List<GarageProfile> _Garages;

        private GarageProfile _selectedGarage;
        public List<GarageProfile> Garages
        {
            get => _Garages;
            set => SetProperty(ref _Garages, value);
        }

        public GarageProfile SelectedGarage
        {
            get => _selectedGarage;
            set
            {
                if (SetProperty(ref _selectedGarage, value))
                {
                    GarageId = value?.Id ?? 0;
                }
            }
        }
        private int _selectedGarageId;
        public int GarageId
        {
            get => _selectedGarageId;
            set
            {
                SetProperty(ref _selectedGarageId, value);
                // If you need to find the full country object:
                SelectedGarage = Garages?.FirstOrDefault(c => c.Id == value);

            }
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
        public async Task SaveVehileAppointment()
        { 
        }
        private async Task LoadGarages()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _apiService.GetGaragesAsync();

                if (apiResponse.IsSuccess)
                {
                    Garages = apiResponse.Data;
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
        private async Task LoadVehicle()
        {
            // Get current user ID from your authentication system
            var response = await _apiService.GetVehicleByID(VehicleId);
            if (response.IsSuccess)
            {
                Vehicle = response.Data;
            }
        }
    }
}
