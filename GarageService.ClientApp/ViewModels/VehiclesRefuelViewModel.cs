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
    [QueryProperty(nameof(VehicleId), "vehileid")]
    public class VehiclesRefuelViewModel: BaseViewModel
    {
        public VehiclesRefuelViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoadVehileCommand = new Command(async () => await LoadVehicle());
            SaveCommand = new Command(async () => await SaveVehileRefule());
            BackCommand = new Command(async () => await GoBack());
            LoadVehileCommand.Execute(null);
        }
        private readonly ApiService _apiService;
        public ICommand LoadVehileCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
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
        private async Task LoadVehicle()
        {
            // Get current user ID from your authentication system
            var response = await _apiService.GetVehicleByID(VehicleId);
            if (response.IsSuccess)
            {
                Vehicle = response.Data;
            }
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
        public async Task SaveVehileRefule()
        {
            var vehiclesRefuel = new VehiclesRefuel
            {
                Vehicleid = VehicleId,
                RefuleValue = RefuelValue,
                RefuelCost = RefuelCost,
                Ododmeter = Odometer,
                RefuleDate = DateTime.Now
            };
            var response = await _apiService.AddVehiclesRefuleAsync(vehiclesRefuel);
            if (response.IsSuccess)
            {
                // Handle success, e.g., show a message or navigate back
                await Shell.Current.DisplayAlert("Success", "Data saved successfully", "OK");
                await Shell.Current.GoToAsync(".."); 
            }
            else
            {
                // Handle error, e.g., show an error message
                await Shell.Current.DisplayAlert("Error", "Failed to save Data", "OK");
            }
        }
        public int Odometer { get; set; }
        public decimal RefuelValue { get; set; }
        public decimal RefuelCost { get; set; }

        private int _vehileid;
        public int VehicleId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
            }
        }
    }

}
