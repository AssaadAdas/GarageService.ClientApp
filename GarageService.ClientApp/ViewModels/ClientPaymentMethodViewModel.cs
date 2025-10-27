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
    public class ClientPaymentMethodViewModel: BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly ISessionService _session_service;
        public int Id { get; set; }
        public int Garageid { get; set; }

        public bool IsPrimary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsActive { get; set; }
        public string CardNumber { get; set; } = null!;
        public string CardHolderName { get; set; } = null!;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; } = null!;

        //payment type id
        private int _selectedPaymentTypeId;
        public int PaymentTypeId
        {
            get => _selectedPaymentTypeId;
            set
            {
                SetProperty(ref _selectedPaymentTypeId, value);
                // keep SelectedPaymentType in sync if the id is set programmatically
                SelectedPaymentType = PaymentTypes?.FirstOrDefault(c => c.Id == value);
            }
        }

        // Months collection (01, 02, ..., 12)
        public ObservableCollection<string> Months { get; } = new ObservableCollection<string>();

        // Years collection (current year + next 10 years)
        public ObservableCollection<string> Years { get; } = new ObservableCollection<string>();
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }

        private List<PaymentType> _PaymentType;
        public List<PaymentType> PaymentTypes
        {
            get => _PaymentType;
            set => SetProperty(ref _PaymentType, value);
        }

        private PaymentType _selectedPaymentType;
        public PaymentType SelectedPaymentType
        {
            get => _selectedPaymentType;
            set
            {
                if (SetProperty(ref _selectedPaymentType, value))
                {
                    PaymentTypeId = value?.Id ?? 0;
                }
            }
        }

        private void InitializeMonths()
        {
            // Add months as two-digit strings
            for (int i = 1; i <= 12; i++)
            {
                Months.Add(i.ToString("D2")); // "01", "02", ..., "12"
            }
        }

        private void InitializeYears()
        {
            int currentYear = DateTime.Now.Year;

            // Add current year and next 10 years
            for (int i = 0; i <= 10; i++)
            {
                Years.Add((currentYear + i).ToString());
            }
        }


        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ClientPaymentMethodViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _apiService = apiservice;
            _session_service = sessionService;
            BackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await SavePaymentMethod());
            InitializeMonths();
            InitializeYears();
            LoadPaymentTypes();
        }
        private async void LoadPaymentTypes()
        {
            try
            {
                ErrorMessage = string.Empty;
                var apiResponse = await _apiService.GetPaymentTypesAsync();

                if (apiResponse.IsSuccess)
                {
                    PaymentTypes = apiResponse.Data;
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
        public async Task SavePaymentMethod()
        {
            if (string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(CardHolderName) ||
                 string.IsNullOrWhiteSpace(Cvv) || ExpiryMonth == 0 || ExpiryYear == 0 || PaymentTypeId == 0)
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }
            // Implementation for saving payment order
            var ClientPaymentMethod = new ClientPaymentMethod
            {
                Clientid = _session_service.ProfileId,
                PaymentTypeId = PaymentTypeId,
                IsPrimary = IsPrimary,
                CardNumber = CardNumber,
                CardHolderName = CardHolderName,
                ExpiryMonth = ExpiryMonth,
                ExpiryYear = ExpiryYear,
                Cvv = Cvv
            };
            var Response = await _apiService.AddClientPaymentMethod(ClientPaymentMethod);
            if (Response.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "Payment Method Saved successful", "OK");
                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Internal error", "OK");
            }
        }
    }
}
