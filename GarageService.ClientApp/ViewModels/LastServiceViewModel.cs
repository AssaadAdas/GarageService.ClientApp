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
    public class LastServiceViewModel: BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadLastServiceCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SelectOfferCommand { get; }

        private VehiclesService _VehiclesService;
        public VehiclesService VehiclesService
        {
            get => _VehiclesService;
            set => SetProperty(ref _VehiclesService, value);
        }

        public LastServiceViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _ApiService = apiservice;
            _sessionService = sessionService;
            LoadLastServiceCommand = new Command(async () => await LoadLastService());
            SaveCommand = new Command(async () => await GoBack());
            BackCommand = new Command(async () => await GoBack());

        }
        private int _vehicleId;
        public int VehicleId
        {
            get => _vehicleId;
            set
            {
                _vehicleId = value;
                OnPropertyChanged(nameof(VehicleId));
                LoadLastServiceCommand.Execute(null);
            }
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
        private async Task LoadLastService()
        {
            try
            {
                IsBusy = true;
                var response = await _ApiService.GetVehicleLastService(VehicleId);
                if (response.IsSuccess)
                {
                    VehiclesService = response.Data;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                // Handle error (show alert, etc.)
                await Shell.Current.DisplayAlert("Error", $"Failed to load Last service: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
