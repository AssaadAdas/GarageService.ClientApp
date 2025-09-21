using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "vehileid")]
    public  class ServicesTypeSetUpViewModel : BaseViewModel
    {
        public List<ServicesTypeSetUp> seviviceTypesetup { get; set; }
        private int _vehileid;
        public int VehicleId
        {
            get => _vehileid;
            set
            {
                _vehileid = value;
                LoadServiceTypesVehicleAsync();
            }
        }
        private ObservableCollection<SelectableServiceTypeSetUpViewModel> _availableServiceTypes;
        public ObservableCollection<SelectableServiceTypeSetUpViewModel> AvailableServiceTypes
        {
            get => _availableServiceTypes;
            set => SetProperty(ref _availableServiceTypes, value);
        }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand LoadSTSCommand { get; }
        public ICommand BackCommand { get; }
        private readonly ApiService _apiService;
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public ServicesTypeSetUpViewModel(ApiService apiService)
        {
            _apiService = apiService;
            SaveCommand = new Command(async () => await SaveAsync());
            LoadCommand = new Command(async () => await LoadServiceTypesAsync());
            BackCommand = new Command(async () => await GoBack());
            LoadCommand.Execute(null);
            _ = LoadMeasureUnitsAsync();
        }
        private async Task SaveAsync()
        {
            try
            {
                var selected = AvailableServiceTypes.ToList();
                if (selected != null)
                {
                    foreach (var setup in selected)
                    {
                        var existing = seviviceTypesetup.FirstOrDefault(s => s.ServiceTypesid == setup.Id);
                        var serviceSetUps = new ServicesTypeSetUp
                        {
                            Id = existing != null ? existing.Id : 0,
                            Vehicleid = VehicleId,
                            ServiceTypesid = setup.Id,
                            ServiceTypesValue = setup.ServiceTypesValue,
                            MeassureUnitid = setup.MeassureUnitid
                        };

                        if (existing != null)
                        {
                            // Update existing record
                            var response = await _apiService.UpdateServicesTypeSetUpAsync(existing.Id, serviceSetUps);
                            if (!response)
                            {
                                await Shell.Current.DisplayAlert("Error", "Failed to update Data", "OK");
                            }
                        }
                        else
                        {
                            // Add new record
                            var response = await _apiService.AddServicesTypeSetUpAsync(serviceSetUps);
                            if (response.IsSuccess)
                            {
                                await Shell.Current.DisplayAlert("Success", "Data saved successfully", "OK");
                                await Shell.Current.GoToAsync("..");
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", "Failed to save Data", "OK");
                            }
                        }
                    }
                    await Shell.Current.GoToAsync($"..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to save Data ", "OK");
            }
        }

        private ObservableCollection<MeassureUnit> _MeassureUnits;
        public ObservableCollection<MeassureUnit> MeassureUnits
        {
            get => _MeassureUnits;
            set => SetProperty(ref _MeassureUnits, value);
        }
        private async Task LoadMeasureUnitsAsync()
        {
            var response = await _apiService.GetMeassureUnitsAsync();
            if (response.IsSuccess)
                MeassureUnits = new ObservableCollection<MeassureUnit>(response.Data);
        }
        private async Task LoadServiceTypesAsync()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _apiService.GetServiceTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    AvailableServiceTypes = new ObservableCollection<SelectableServiceTypeSetUpViewModel>(
                                     apiResponse.Data.Select(st => new SelectableServiceTypeSetUpViewModel(st)));
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

        private async Task LoadServiceTypesVehicleAsync()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _apiService.GetServicesTypeSetUpVehicleAsync(_vehileid);
                if (apiResponse.IsSuccess)
                {
                    seviviceTypesetup = apiResponse.Data;
                    var existingSetups = apiResponse.Data;
                    if (AvailableServiceTypes != null)
                    {
                        foreach (var setup in existingSetups)
                        {
                            var matchingItem = AvailableServiceTypes.FirstOrDefault(st => st.Id == setup.ServiceTypesid);
                            if (matchingItem != null)
                            {
                                matchingItem.ServiceTypesValue = setup.ServiceTypesValue;
                                matchingItem.MeassureUnitid = setup.MeassureUnitid;
                                matchingItem.SelectedMeassureUnit = MeassureUnits?.FirstOrDefault(mu => mu.Id == setup.MeassureUnitid);
                            }
                        }
                    }
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
            await Shell.Current.GoToAsync($"..");
        }
    }
}
