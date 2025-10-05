using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
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
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;


namespace GarageService.ClientApp.ViewModels
{
    
    public class VehiclesServiceTypeViewModel:BaseViewModel
    {
        private ObservableCollection<SelectableServiceTypeViewModel> _availableServiceTypes;
        public ObservableCollection<SelectableServiceTypeViewModel> AvailableServiceTypes
        {
            get => _availableServiceTypes;
            set => SetProperty(ref _availableServiceTypes, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand BackCommand { get; }
        private readonly ApiService _apiService;
        ServiceFormState _formState;
        public List<SelectableServiceTypeViewModel> SelectedServiceTypes { get; set; } = new();

        public VehiclesServiceTypeViewModel(ApiService apiService, ServiceFormState formState)
        {
            _apiService = apiService;
            _formState = formState;
            _previouslySelectedServiceTypes = _formState.SelectedServiceTypes;
            SaveCommand = new Command(async () => await OnDone());
            LoadCommand = new Command(async () => await LoadServiceTypesAsync());
            BackCommand = new Command(async () => await GoBack());
            _ = LoadCurrenciesAsync();
            LoadCommand.Execute(null);
            
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ServicePage)}");
        }
        private ObservableCollection<SelectableServiceTypeViewModel> _previouslySelectedServiceTypes;

      
        private ObservableCollection<Currency> _currencies;
        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set => SetProperty(ref _currencies, value);
        }

        // In your constructor or LoadCommand, call this:
        private async Task LoadCurrenciesAsync()
        {
            var response = await _apiService.GetCurremciesAsync();
            if (response.IsSuccess)
                Currencies = new ObservableCollection<Currency>(response.Data);
        }

        #region load data
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        private async Task LoadServiceTypesAsync()
        {
            try
            {
                ErrorMessage = string.Empty;

                var apiResponse = await _apiService.GetServiceTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    AvailableServiceTypes = new ObservableCollection<SelectableServiceTypeViewModel>(
                    apiResponse.Data.Select(st =>
                    {
                        var vm = new SelectableServiceTypeViewModel(st);
                        // Restore IsSelected and Cost if previously selected
                        var prev = _previouslySelectedServiceTypes?.FirstOrDefault(x => x.Id == st.Id);
                        if (prev != null)
                        {
                            vm.IsSelected = true;
                            vm.Cost = prev.Cost;
                            vm.Notes = prev.Notes;
                            vm.CurrId = prev.CurrId;
                            
                            vm.CurrDesc = prev.CurrDesc;

                            vm.CurrId = prev.CurrId;
                            // Set SelectedCurrency if Currencies are loaded
                            if (Currencies != null)
                                vm.SelectedCurrency = Currencies.FirstOrDefault(c => c.Id == prev.CurrId);
                        }
                        return vm;
                    }));
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
        #endregion
        private async Task OnDone()
        {
            try
            {
                var selected = AvailableServiceTypes.Where(x => x.IsSelected).ToList();
                if (selected.Count == 0)
                {
                    ErrorMessage = "Please select at least one service type.";
                    return;
                }

                var navigationParams = new Dictionary<string, object>
                 {
                      { "SelectedServiceTypes", selected }
                 };

                await Shell.Current.GoToAsync("ServicePage", navigationParams);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to save selections";
                Debug.WriteLine($"OnDone error: {ex}");
            }
        }

       
    }
}
