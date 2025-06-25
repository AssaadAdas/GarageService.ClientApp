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
    public class AddVehicleViewModel: BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        private ClientProfile _clientProfile;
        private int _vehileid;
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        public int VehileId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
                LoadCommand.Execute(null);
            }
        }
        private List<VehicleType> _vehicletypes;
        private VehicleType _selectedVehicleType;
        public List<VehicleType> vehicletypes
        {
            get => _vehicletypes;
            set => SetProperty(ref _vehicletypes, value);
        }

        public VehicleType SelectedVehicleType
        {
            get => _selectedVehicleType;
            set
            {
                if (SetProperty(ref _selectedVehicleType, value))
                {
                    VehicleTypeId = value?.Id ?? 0;
                }
            }
        }

        private List<Manufacturer> _manufacturers;
        private Manufacturer _selectedmanufacturers;

        public List<Manufacturer> manufacturers
        {
            get => _manufacturers;
            set => SetProperty(ref _manufacturers, value);
        }

        public Manufacturer SelectedManufacturer
        {
            get => _selectedmanufacturers;
            set
            {
                if (SetProperty(ref _selectedmanufacturers, value))
                {
                    ManufacturerId = value?.Id ?? 0;
                }
            }
        }

        private List<FuelType> _fueltypes;
        private FuelType _selectedFuelTypes;
        public List<FuelType> fueltypes
        {
            get => _fueltypes;
            set => SetProperty(ref _fueltypes, value);
        }

        public FuelType Selectedfueltype
        {
            get => _selectedFuelTypes;
            set
            {
                if (SetProperty(ref _selectedFuelTypes, value))
                {
                    FuelTypeId = value?.Id ?? 0;
                }
            }
        }

        private List<MeassureUnit> _meassureunits;
        private MeassureUnit _selectedmeassureunit;

        public List<MeassureUnit> meassureunits
        {
            get => _meassureunits;
            set => SetProperty(ref _meassureunits, value);
        }

        public MeassureUnit selectedmeassureunit
        {
            get => _selectedmeassureunit;
            set
            {
                if (SetProperty(ref _selectedmeassureunit, value))
                {
                    MeassureUnitId = value?.Id ?? 0;
                }
            }
        }

        public AddVehicleViewModel(ApiService apiService, ISessionService sessionService)
        {
            _ApiService = apiService;
            _sessionService = sessionService;
            SaveCommand = new Command(async () => await SaveVehile());
            LoadClientProfile();
            LoadVehiclesTypes();
            LoadManufacturers();
            LoadFuelTypes();
            LoadMeasureUnits();
        }
        #region 
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private async void LoadMeasureUnits()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetMeassureUnitsAsync();

                if (apiResponse.IsSuccess)
                {
                    meassureunits = apiResponse.Data;
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

        private async void LoadFuelTypes()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetFuelTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    fueltypes = apiResponse.Data;
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

        private async void LoadManufacturers()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetManufacturersAsync();

                if (apiResponse.IsSuccess)
                {
                    manufacturers = apiResponse.Data;
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

        private async void LoadVehiclesTypes()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _ApiService.GetVehicleTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    vehicletypes = apiResponse.Data;
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
        public int VehicleTypeId { get; set; }

        public string VehicleName { get; set; } = null!;

        public int ManufacturerId { get; set; }

        public string Model { get; set; } = null!;

        public string LiscencePlate { get; set; } = null!;

        public int FuelTypeId { get; set; }

        public string ChassisNumber { get; set; } = null!;

        public int MeassureUnitId { get; set; }

        public string? Identification { get; set; }

        public bool Active { get; set; }

        public int ClientId { get; set; }

        public int Odometer { get; set; }
        #endregion
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


        public async Task SaveVehile()
        {
            if (string.IsNullOrWhiteSpace(VehicleName))
            {
                await Shell.Current.DisplayAlert("Error", "VehicleName is required fields", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Model))
            {
                await Shell.Current.DisplayAlert("Error", "Model is required fields", "OK");
                return;
            }
            var newvehicle = new Vehicle
            {
                VehicleTypeId = VehicleTypeId,
                VehicleName = VehicleName,
                ManufacturerId = ManufacturerId,
                Model = Model,
                LiscencePlate = LiscencePlate,
                FuelTypeId = FuelTypeId,
                ChassisNumber = ChassisNumber,
                MeassureUnitId = MeassureUnitId,
                Identification = Identification,
                Active = Active,
                ClientId = ClientProfile.Id, // Assuming ClientProfile is already loaded
                Odometer = Odometer
            };

            var VehicleAddedResponse = await _ApiService.AddVehicleAsync(newvehicle);
            if (VehicleAddedResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "Vehicle added successfully", "OK");
                // Optionally, navigate back or clear the form
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", VehicleAddedResponse.Message, "OK");
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
