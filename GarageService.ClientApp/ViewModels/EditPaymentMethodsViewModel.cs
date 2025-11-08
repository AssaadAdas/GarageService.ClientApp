using GarageService.ClientApp.Views;
using GarageService.ClientLib.Models;
using GarageService.ClientLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GarageService.ClientApp.ViewModels
{
    [QueryProperty(nameof(PaymentMethodId), "paymentMethodId")]
    public class EditPaymentMethodsViewModel:BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;

        public ICommand BackCommand { get; }
        public ICommand LoadPaymentMethodCommand { get; }
        public ICommand LoadClientCommand { get; }
        public ICommand SaveCommand { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        private bool _isPrimary;
        public bool IsPrimary
        {
            get => _isPrimary;
            set => SetProperty(ref _isPrimary, value);
        }

        private string _cardNumber = string.Empty;
        public string CardNumber
        {
            get => _cardNumber;
            set => SetProperty(ref _cardNumber, value);
        }

        private string _cardHolderName = string.Empty;
        public string CardHolderName
        {
            get => _cardHolderName;
            set => SetProperty(ref _cardHolderName, value);
        }

        private int _expiryMonth;
        public int ExpiryMonth
        {
            get => _expiryMonth;
            set => SetProperty(ref _expiryMonth, value);
        }

        private int _expiryYear;
        public int ExpiryYear
        {
            get => _expiryYear;
            set => SetProperty(ref _expiryYear, value);
        }

        private string _cvv = string.Empty;
        public string Cvv
        {
            get => _cvv;
            set => SetProperty(ref _cvv, value);
        }

        private ClientProfile _clientProfile;
        public ClientProfile ClientProfile
        {
            get => _clientProfile;
            set
            {
                if (_clientProfile != value)
                {
                    _clientProfile = value;
                    OnPropertyChanged(nameof(ClientProfile));
                }
            }
        }

        private int _PaymentMethodId;
        public int PaymentMethodId
        {
            get => _PaymentMethodId;
            set
            {
                if (SetProperty(ref _PaymentMethodId, value))
                {
                    // fire-and-forget load; avoids blocking setter
                    _ = LoadPaymentMethodAsync();
                }
            }
        }

        private ClientPaymentMethod _ClientPaymentMethod = new();
        public ClientPaymentMethod ClientPaymentMethod
        {
            get => _ClientPaymentMethod;
            set
            {
                if (_ClientPaymentMethod != value)
                {
                    _ClientPaymentMethod = value;
                    OnPropertyChanged(nameof(ClientPaymentMethod));
                }
            }
        }


        public ObservableCollection<int> Months { get; } = new ObservableCollection<int>();

        // Years collection as ints (matches ExpiryYear type)
        public ObservableCollection<int> Years { get; } = new ObservableCollection<int>();

        private void InitializeMonths()
        {
            // Add months as two-digit strings
            for (int i = 1; i <= 12; i++)
            {
                Months.Add(i); // "01", "02", ..., "12"
            }
        }

        private void InitializeYears()
        {
            int currentYear = DateTime.Now.Year;

            // Add current year and next 10 years
            for (int i = 0; i <= 10; i++)
            {
                Years.Add((currentYear + i));
            }
        }

        public EditPaymentMethodsViewModel(ApiService apiService, ISessionService sessionService)
        {
            _ApiService = apiService;
            _sessionService = sessionService;
            SaveCommand = new Command(async () => await SavePaymentMethod());
            BackCommand = new Command(async () => await GoBack());
            LoadClientCommand = new Command(async () => await LoadClientProfile());
            LoadPaymentMethodCommand = new Command(async () => await LoadPaymentMethodAsync());
            LoadClientCommand.Execute(null);
            InitializeMonths();
            InitializeYears();

        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}");
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public async Task LoadPaymentMethodAsync()
        {
            var response = await _ApiService.GetPaymentMethodID(PaymentMethodId);
            ClientPaymentMethod = response.Data;
            if (ClientPaymentMethod != null)
            {
                IsActive = ClientPaymentMethod.IsActive;

                // assign using SetProperty-backed properties so UI updates
                CardNumber = ClientPaymentMethod.CardNumber ?? string.Empty;
                CardHolderName = ClientPaymentMethod.CardHolderName ?? string.Empty;

                // ensure month/year exist in collections then set
                ExpiryMonth = ClientPaymentMethod.ExpiryMonth;
                if (!Months.Contains(ExpiryMonth) && ExpiryMonth >= 1 && ExpiryMonth <= 12)
                    Months.Add(ExpiryMonth);

                ExpiryYear = ClientPaymentMethod.ExpiryYear;
                if (!Years.Contains(ExpiryYear) && ExpiryYear > 0)
                    Years.Add(ExpiryYear);
                IsActive = ClientPaymentMethod.IsActive;
                IsPrimary = ClientPaymentMethod.IsPrimary;
                Cvv = ClientPaymentMethod.Cvv ?? string.Empty;
            }
        }

        public async Task LoadClientProfile()
        {
            // Get current user ID from your authentication system
            int ClientId = GetCurrentUserId();

            var response = await _ApiService.GetClientByID(ClientId);
            if (response.IsSuccess)
            {
                ClientProfile = response.Data;
            }
        }
        private int GetCurrentUserId()
        {
            // Implement your actual user ID retrieval logic
            int userId;
            string userType;
            int profileId = 1;
            if (_sessionService.IsLoggedIn)
            {
                userId = _sessionService.UserId;
                userType = _sessionService.UserType.ToString();
                profileId = _sessionService.ProfileId;
            }
            else
            {
                // If not logged in, you might want to redirect to login page or throw an exception
                throw new UnauthorizedAccessException("User is not logged in.");
            }

            return profileId; // Placeholder
        }

        public async Task SavePaymentMethod()
        {
            if (string.IsNullOrWhiteSpace(CardHolderName))
            {
                await Shell.Current.DisplayAlert("Error", "CardHolderName is required fields", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(Cvv))
            {
                await Shell.Current.DisplayAlert("Error", "Cvv is required fields", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(CardNumber))
            {
                await Shell.Current.DisplayAlert("Error", "CardNumber is required fields", "OK");
                return;
            }

            if (ExpiryMonth==0)
            {
                await Shell.Current.DisplayAlert("Error", "ExpiryMonth is required fields", "OK");
                return;
            }

            if (ExpiryYear == 0)
            {
                await Shell.Current.DisplayAlert("Error", "ExpiryYear is required fields", "OK");
                return;
            }
            ClientPaymentMethod.Clientid = ClientProfile.Id;
            ClientPaymentMethod.LastModified = DateTime.Now;
            ClientPaymentMethod.CardNumber = CardNumber;
            ClientPaymentMethod.CardHolderName = CardHolderName;
            ClientPaymentMethod.ExpiryMonth = ExpiryMonth;
            ClientPaymentMethod.ExpiryYear = ExpiryYear;
            ClientPaymentMethod.IsPrimary = ClientPaymentMethod.IsPrimary;
            ClientPaymentMethod.IsActive = IsActive;
            ClientPaymentMethod.IsPrimary = IsPrimary;
            ClientPaymentMethod.PaymentTypeId = ClientPaymentMethod.PaymentTypeId;
            ClientPaymentMethod.Cvv = Cvv;
            ClientPaymentMethod.Client =ClientProfile;

            bool success = await _ApiService.UpdatePaymentMethodAsync(ClientPaymentMethod.Id, ClientPaymentMethod);

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Payment method updated.", "OK");
                await Shell.Current.GoToAsync("..");
            }
                
            else
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update payment method.", "OK");
            
        }
    }
}
