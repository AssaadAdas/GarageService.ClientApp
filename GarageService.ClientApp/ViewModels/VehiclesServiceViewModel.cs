using CommunityToolkit.Mvvm.Messaging;
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
    public class VehiclesServiceViewModel:BaseViewModel
    {
        public VehiclesServiceViewModel(ApiService apiService)
        {
            _apiService = apiService;
            AddServiceTypeCommand = new Command(async () => await AddServiceTypes());
            SaveCommand = new Command(async () => await SaveService());
            LoadGargesCommand = new Command(async () => await LoadGarages());
            LoadGargesCommand.Execute(null);
        }

        private VehiclesService _service;
        private ObservableCollection<VehiclesServiceTypeViewModel> _serviceTypes;
        private readonly ApiService _apiService;
        public ICommand LoadServicesTypesCommand { get; }
        public ICommand AddServiceTypeCommand { get;}
        public ICommand SaveCommand { get; }
        public ICommand LoadGargesCommand { get; }

        private List<GarageProfile> _Garages;
        private GarageProfile _selectedGarage;
        public List<GarageProfile> Garages
        {
            get => _Garages;
            set => SetProperty(ref _Garages, value);
        }

        private ObservableCollection<ServiceType> _selectedServiceTypes;
        public ObservableCollection<ServiceType> SelectedServiceTypes
        {
            get => _selectedServiceTypes;
            set
            {
                _selectedServiceTypes = value;
                OnPropertyChanged(nameof(SelectedServiceTypes));
            }
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


        public DateTime ServiceDate { get; set; }

        public int Odometer { get; set; }

        public string ServiceLocation { get; set; }

        public string Notes { get; set; }

        public int VehicleId { get; set; }

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

        private async Task AddServiceTypes()
        {
            //var navigationParams = new ShellNavigationQueryParameters
            //{
            //    { "PreviouslySelectedItems", SelectedServiceTypes.ToList() }
            //};
            await Shell.Current.GoToAsync($"{nameof(AddServiceTypePage)}");
            //await Shell.Current.GoToAsync(nameof(AddServiceTypePage), navigationParams);
        }
        public void UpdateSelectedItems(List<ServiceType> items)
        {
            SelectedServiceTypes.Clear();
            foreach (var item in items)
            {
                SelectedServiceTypes.Add(item);
            }
        }
        private async Task SaveService()
        {
            //await Shell.Current.GoToAsync($"{nameof(AddServiceTypePage)}");
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
