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
    public class EditVehicleViewModel: BaseViewModel
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
        
        public ICommand LoadCommand { get; }
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

        public EditVehicleViewModel(ApiService apiService, ISessionService sessionService)
        {
            _ApiService = apiService;
            _sessionService = sessionService;
            SaveCommand = new Command(async () => await SaveVehile());
            LoadClientProfile();
            LoadVehiclesTypes();
            LoadManufacturers();
            LoadFuelTypes();
            LoadMeasureUnits();
            LoadCommand = new Command(async () => await LoadVehicle());
            LoadCommand.Execute(null);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

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
                    VehicleName = Vehicle.VehicleName;
                    Odometer = Vehicle.Odometer;
                    Identification = Vehicle.Identification;
                    Active = Vehicle.Active;
                    VehicleTypeId = Vehicle.VehicleTypeId;
                    ManufacturerId = Vehicle.ManufacturerId;
                    ClientId = Vehicle.ClientId;
                    Model = Vehicle.Model;
                    LiscencePlate = Vehicle.LiscencePlate;
                    FuelTypeId = Vehicle.FuelTypeId;
                    ChassisNumber = Vehicle.ChassisNumber;
                    MeassureUnitId = Vehicle.MeassureUnitId;
                    
                    // Assuming ClientProfile is already loaded, you can set the properties directly

                    //// Set the selected country based on the CountryId
                    SelectedManufacturer = manufacturers?.FirstOrDefault(m => m.Id == ManufacturerId);
                    selectedmeassureunit = meassureunits?.FirstOrDefault(mu => mu.Id == MeassureUnitId);
                    Selectedfueltype = fueltypes?.FirstOrDefault(ft => ft.Id == FuelTypeId);
                    SelectedVehicleType = vehicletypes?.FirstOrDefault(vt => vt.Id == VehicleTypeId);

                    OnPropertyChanged(nameof(VehicleName));
                    OnPropertyChanged(nameof(Odometer));
                    OnPropertyChanged(nameof(Identification));
                    OnPropertyChanged(nameof(Active));
                    OnPropertyChanged(nameof(Model));
                    OnPropertyChanged(nameof(LiscencePlate));

                    OnPropertyChanged(nameof(ChassisNumber));
                    OnPropertyChanged(nameof(SelectedManufacturer));
                    OnPropertyChanged(nameof(selectedmeassureunit));
                    OnPropertyChanged(nameof(Selectedfueltype));
                    OnPropertyChanged(nameof(SelectedVehicleType));
                }
            }
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
            Vehicle.VehicleTypeId = VehicleTypeId;
            Vehicle.VehicleName = VehicleName;
            Vehicle.ManufacturerId = ManufacturerId;
            Vehicle.Model = Model;
            Vehicle.LiscencePlate = LiscencePlate;
            Vehicle.FuelTypeId = FuelTypeId;
            Vehicle.ChassisNumber = ChassisNumber;
            Vehicle.MeassureUnitId = MeassureUnitId;
            Vehicle.Identification = Identification;
            Vehicle.Active = Active;
            Vehicle.ClientId = ClientProfile.Id; // Assuming ClientProfile is already loaded
            Vehicle.Odometer = Odometer;

            bool success = await _ApiService.UpdateVehicleAsync(Vehicle.Id, Vehicle);
            if (success)
            {
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
