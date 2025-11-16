using CommunityToolkit.Mvvm.Messaging;
using GarageService.ClientApp.Services;
using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "vehileid")]
    public class VehiclesServiceViewModel:BaseViewModel, IQueryAttributable
    {
        public VehiclesServiceViewModel(ApiService apiService, ServiceFormState formState)
        {
            _apiService = apiService;
            _formState = formState;
            AddServiceTypeCommand = new Command(async () => await AddServiceTypes());
            SaveCommand = new Command(async () => await SaveService());
            BackCommand = new Command(async () => await GoBack());
            LoadGargesCommand = new Command(async () => await LoadGarages());
            LoadGargesCommand.Execute(null);
            if (_formState.VehicleId != 0)
            {
                VehicleId = _formState.VehicleId;
                Odometer = _formState.Odometer;
                GarageId = _formState.GarageId;
                Notes = _formState.Notes;
                ServiceDate = _formState.ServiceDate;
                SelectedGarage = _formState.selectedGarage;
                ServiceTypess = _formState.ServiceTypes ?? new ObservableCollection<SelectableServiceTypeViewModel>();
                
            }
        }
        
        private ObservableCollection<SelectableServiceTypeViewModel> _ServiceTypess = new();
        public ObservableCollection<SelectableServiceTypeViewModel> ServiceTypess
        {
            get => _ServiceTypess;
            set
            {
                _ServiceTypess = value;
                OnPropertyChanged(nameof(ServiceTypess));
            }
        }

        private void UpdateTotalCost()
        {
            TotalServiceAmount = ServiceTypess?.Sum(x => x.Cost) ?? 0;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            // Handle SelectedServiceTypes - only update if we're coming from AddServiceTypePage
            if (query.TryGetValue("SelectedServiceTypes", out var value) && value is IEnumerable<SelectableServiceTypeViewModel> selected)
            {
                if (ServiceTypess == null)
                {
                    ServiceTypess = new ObservableCollection<SelectableServiceTypeViewModel>(selected);
                }
                else
                {
                    // Remove items not in the new selection
                    var toRemove = ServiceTypess.Where(x => !selected.Any(s => s.Id == x.Id)).ToList();
                    foreach (var item in toRemove)
                        ServiceTypess.Remove(item);
                    //ServiceTypess = new ObservableCollection<SelectableServiceTypeViewModel>(ServiceTypess.ToList());
                    // Add or update items from the new selection
                    decimal TotalServiceAmounts = 0;
                    foreach (var item in selected)
                    {
                        var existing = ServiceTypess.FirstOrDefault(x => x.Id == item.Id);
                        if (existing == null)
                        {
                            ServiceTypess.Add(item);
                            TotalServiceAmounts += item.Cost;
                        }
                        else
                        {
                            // Only update the properties that might have changed in the service types
                            existing.IsSelected = item.IsSelected;
                            existing.Cost = item.Cost;
                            existing.Notes = item.Notes;
                            existing.CurrId = item.CurrId;
                            existing.CurrDesc = item.CurrDesc;
                            TotalServiceAmounts += item.Cost;
                        }
                    }
                    TotalServiceAmount = TotalServiceAmounts;
                    OnPropertyChanged(nameof(TotalServiceAmount));
                }
                OnPropertyChanged(nameof(ServiceTypess));
            }

            // Handle vehicleid
            if (query.TryGetValue("vehicleid", out var vehicleIdValue))
            {
                if (vehicleIdValue is int id)
                {
                    VehicleId = id;
                }
                else if (int.TryParse(vehicleIdValue?.ToString(), out int parsedId))
                {
                    VehicleId = parsedId;
                }
            }
        }

        private readonly ApiService _apiService;
        private readonly ServiceFormState _formState;
        public ICommand LoadServicesTypesCommand { get; }
        public ICommand AddServiceTypeCommand { get;}
        public ICommand SaveCommand { get; }
        public ICommand LoadGargesCommand { get; }
        public ICommand BackCommand { get; }

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
        private DateTime _serviceDate = DateTime.Now; // Default value
        public DateTime ServiceDate
        {
            get => _serviceDate;
            set => SetProperty(ref _serviceDate, value);
        }

        private int _odometer;
        public int Odometer
        {
            get => _odometer;
            set => SetProperty(ref _odometer, value);
        }

        private string _ServiceLocation;
        public string ServiceLocation {
            get => _ServiceLocation;
            set => SetProperty(ref _ServiceLocation, value);
        }
        private string _Notes;
        public string Notes {
            get => _Notes;
            set => SetProperty(ref _Notes, value);
        }

        private int _vehileid;
        public int VehicleId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private decimal _totalServiceAmount;
        public decimal TotalServiceAmount
        {
            get => _totalServiceAmount;
            set => SetProperty(ref _totalServiceAmount, value);
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
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

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
        private async Task AddServiceTypes()
        {
            _formState.VehicleId = VehicleId;
            _formState.Odometer = Odometer;
            _formState.GarageId = GarageId;
            _formState.Notes = Notes;
            _formState.ServiceDate = ServiceDate;
            _formState.ServiceTypes = ServiceTypess;
            _formState.selectedGarage = SelectedGarage;

            _formState.SelectedServiceTypes = new ObservableCollection<SelectableServiceTypeViewModel>(
               ServiceTypess.Where(st => st.IsSelected));


            await Shell.Current.GoToAsync($"{nameof(AddServiceTypePage)}");
        }
       
        private async Task SaveService()
        {
            if (ServiceTypess is null)
            {
                await Shell.Current.DisplayAlert("Error", "Service types required fields", "OK");
                return;
            }
            if (Odometer == 0)
            {
                await Shell.Current.DisplayAlert("Error", "Odometer required fields", "OK");
                return;
            }
            if (GarageId == 0)
            {
                await Shell.Current.DisplayAlert("Error", "Garage required fields", "OK");
                return;
            }
            var vehiclesService = new VehiclesService
            {
               ServiceDate = ServiceDate,
               Odometer = Odometer,
               ServiceLocation = SelectedGarage.GarageLocation,
               Notes = Notes,
               Garageid = GarageId,
               Vehicleid = VehicleId,
            };
            var ApiResponse= await _apiService.AddVehiclesServicesAsync(vehiclesService);
            var addedvehiclesservice = ApiResponse.Data;
            var vehiclesServiceTypes = new List<VehiclesServiceType>();
            foreach (var serviceType in ServiceTypess)
            {
                if (serviceType.IsSelected)
                {
                    vehiclesServiceTypes.Add(new VehiclesServiceType
                    {
                        VehicleServiceId = addedvehiclesservice.Id,
                        ServiceTypeId = serviceType.Id,
                        Cost = serviceType.Cost,
                        Notes = serviceType.Notes,
                        CurrId = serviceType.CurrId
                    });
                }
            }
            foreach (var serviceType in vehiclesServiceTypes)
            {
                var (isSuccess2, message2, addedServiceType) = await _apiService.AddVehiclesServiceTypeAsync(serviceType);
                if (!isSuccess2)
                {
                    await Shell.Current.DisplayAlert("Error", message2, "OK");
                    return;
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
