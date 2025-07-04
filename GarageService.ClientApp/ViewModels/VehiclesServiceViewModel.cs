using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "vehileid")]
    public class VehiclesServiceViewModel:BaseViewModel
    {
        private VehiclesService _service;
        private ObservableCollection<VehiclesServiceTypeViewModel> _serviceTypes;
        private readonly ApiService _apiService;

        private ICommand AddServiceTypeCommand { get;}
        private ICommand SaveCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public VehiclesServiceViewModel(VehiclesService service, ApiService apiService)
        {
            _apiService = apiService;
            _service = service;
            _serviceTypes = new ObservableCollection<VehiclesServiceTypeViewModel>(
                service.VehiclesServiceTypes.Select(st => new VehiclesServiceTypeViewModel(st)));
            AddServiceTypeCommand = new Command(async () => await AddServiceTypes());
            SaveCommand = new Command(async () => await SaveService());
        }

        private async Task AddServiceTypes()
        {
            await Shell.Current.GoToAsync($"{nameof(AddServiceTypePage)}");
        }

        private async Task SaveService()
        {
            //await Shell.Current.GoToAsync($"{nameof(AddServiceTypePage)}");
        }

        public int Id
        {
            get => _service.Id;
            set { _service.Id = value; OnPropertyChanged(); }
        }

        public DateTime ServiceDate
        {
            get => _service.ServiceDate;
            set { _service.ServiceDate = value; OnPropertyChanged(); }
        }

        public int Odometer
        {
            get => _service.Odometer;
            set { _service.Odometer = value; OnPropertyChanged(); }
        }

        public string ServiceLocation
        {
            get => _service.ServiceLocation;
            set { _service.ServiceLocation = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _service.Notes ?? string.Empty;
            set { _service.Notes = value; OnPropertyChanged(); }
        }

        public int VehicleId
        {
            get => _service.Vehicleid;
            set { _service.Vehicleid = value; OnPropertyChanged(); }
        }

        public int GarageId
        {
            get => _service.Garageid;
            set { _service.Garageid = value; OnPropertyChanged(); }
        }

        public ObservableCollection<VehiclesServiceTypeViewModel> ServiceTypes
        {
            get => _serviceTypes;
            set { _serviceTypes = value; OnPropertyChanged(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
