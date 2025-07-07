using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;


namespace GarageService.ClientApp.ViewModels
{
    public class VehiclesServiceTypeViewModel:BaseViewModel
    {
        public ObservableCollection<SelectableServiceTypeViewModel> AvailableServiceTypes { get; set; }
        public ObservableCollection<SelectableServiceTypeViewModel> SelectedServiceTypes { get; set; } 
        public ICommand DoneCommand { get; }
        public ICommand LoadCommand { get; }
        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        private int _currId;
        public int CurrId
        {
            get => _currId;
            set => SetProperty(ref _currId, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        private readonly ApiService _apiService;

        public VehiclesServiceTypeViewModel(ApiService apiService)
        {
            _apiService = apiService;
            DoneCommand = new Command(async () => await OnDone());
            LoadCommand = new Command(async () => await LoadServiceTypesAsync());
            LoadCommand.Execute(null);
        }

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
                                     apiResponse.Data.Select(st => new SelectableServiceTypeViewModel(st)));
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
        private async Task OnDone()
        {
            try
            {
                // Clear previous selections
                SelectedServiceTypes?.Clear();

                // Check if any services are available
                if (AvailableServiceTypes == null || AvailableServiceTypes.Count == 0)
                {
                    ErrorMessage = "No service types available";
                    return;
                }

                if (SelectedServiceTypes == null)
                {
                    SelectedServiceTypes = new ObservableCollection<SelectableServiceTypeViewModel>();
                }

                var selected = AvailableServiceTypes.Where(x => x.IsSelected).ToList();
                foreach (var service in selected)
                {
                    SelectedServiceTypes.Add(service);
                }
                
               var navigationParams = new Dictionary<string, object>
               {
                    { "SelectedServiceTypes", SelectedServiceTypes }
               };

                await Shell.Current.GoToAsync("..", navigationParams);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to save selections";
                Debug.WriteLine($"OnDone error: {ex}");
            }
        }
    }
}
