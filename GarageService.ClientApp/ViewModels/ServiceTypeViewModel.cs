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
    public class ServiceTypeViewModel:BaseViewModel
    {
        private readonly ApiService _apiService;
        public ICommand LoadCommand { get; }
        public ServiceTypeViewModel(ApiService apiService)
        {
            _apiService = apiService;
            LoadCommand = new Command(async () => await LoadServiceTypesAsync());
            LoadCommand.Execute(null);
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        private List<ServiceType> _ServiceTypes;
        public List<ServiceType> AvailableServiceTypes
        {
            get => _ServiceTypes;
            set => SetProperty(ref _ServiceTypes, value);
        }
        private async Task LoadServiceTypesAsync()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _apiService.GetServiceTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    AvailableServiceTypes = apiResponse.Data;
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
    }
}
