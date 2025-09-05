using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "vehicleid")]
    public class VehicleHistoryViewModel: BaseViewModel
    {
        private int _vehicleId;
        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged(nameof(Vehicle));
                LoadHistoryCommand.Execute(null);
            }
        }
        private VehicleHistoryResponse _history;
        public VehicleHistoryResponse History
        {
            get => _history;
            set => SetProperty(ref _history, value);
        }

        private List<VehicleAppointment> _vehicleAppointment;
        public List<VehicleAppointment> VehicleAppointments
        {
            get => _vehicleAppointment;
            set => SetProperty(ref _vehicleAppointment, value);
        }

        private List<VehiclesService> _VehiclesService;
        public List<VehiclesService> VehiclesServices
        {
            get => _VehiclesService;
            set => SetProperty(ref _VehiclesService, value);
        }

        private List<ServiceType> _ServiceType;
        public List<ServiceType> ServiceTypes
        {
            get => _ServiceType;
            set => SetProperty(ref _ServiceType, value);
        }

        private List<VehiclesServiceType> _vehicleServiceType;
        public List<VehiclesServiceType> VehicleServiceTypes
        {
            get => _vehicleServiceType;
            set => SetProperty(ref _vehicleServiceType, value);
        }

        private List<ServiceHistory> _ServiceHistory;
        public List<ServiceHistory> ServiceHistory
        {
            get => _ServiceHistory;
            set => SetProperty(ref _ServiceHistory, value);
        }
        public ICommand LoadHistoryCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        private readonly ApiService _apiService;
        public VehicleHistoryViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoadHistoryCommand = new Command(async () => await LoadHistory());
            SaveCommand = new Command(async () => await GoBack());
            BackCommand = new Command(async () => await GoBack());
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
        private async Task LoadHistory()
        {
            try
            {
                IsBusy = true;
                History = await _apiService.GetVehicleHistory(VehicleId);
                VehicleAppointments = History.Appointments;
                VehiclesServices = History.Services;
                foreach(var service in VehiclesServices)
                {
                    // put my code here
                    foreach (var VehicleserviceType in service.VehiclesServiceTypes)
                    {
                        var servicetype = VehicleserviceType.ServiceType;
                        var desc = servicetype.Description;
                        var serviceHistory = new ServiceHistory { 
                            Description = desc,
                            ServiceDate = service.ServiceDate,
                            Odometer = service.Odometer,
                            Notes = VehicleserviceType.Notes
                            };
                        if (ServiceHistory == null)
                        {
                            ServiceHistory = new List<ServiceHistory>();
                        }
                        ServiceHistory.Add(serviceHistory);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error (show alert, etc.)
                await Shell.Current.DisplayAlert("Error", $"Failed to load history: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
