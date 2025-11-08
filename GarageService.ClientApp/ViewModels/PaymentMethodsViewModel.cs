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
    public class PaymentMethodsViewModel:BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly ISessionService _sessionService;
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadPaymentMethodCommand { get; }
        public ICommand AddPaymentMethodCommand { get; }
        public ICommand EditPaymentMethodCommand { get; }

        public PaymentMethodsViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _apiService = apiservice;
            _sessionService = sessionService;
            LoadPaymentMethodCommand = new Command(async () => await LoadPaymentMethods());
            AddPaymentMethodCommand = new Command(async () => await AddPaymentMethod());
            EditPaymentMethodCommand = new Command<ClientPaymentMethod>(async (ClientPaymentMethod) => await EditPaymentMethod(ClientPaymentMethod));
            BackCommand = new Command(async () => await GoBack());
            LoadClientProfile();
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private async Task AddPaymentMethod()
        {
            await Shell.Current.GoToAsync(nameof(ClientPaymentMethodPage));
        }

        private async Task EditPaymentMethod(ClientPaymentMethod ClientPaymentMethod)
        {
            await Shell.Current.GoToAsync($"{nameof(EditPaymentMethodsPage)}?paymentMethodId={ClientPaymentMethod.Id}");
        }

        private List<ClientPaymentMethod> _clientPaymentMethod;
        public List<ClientPaymentMethod> ClientPaymentMethod
        {
            get => _clientPaymentMethod;
            set
            {
                if (_clientPaymentMethod != value)
                {
                    _clientPaymentMethod = value;
                    OnPropertyChanged(nameof(ClientPaymentMethod));
                }
            }
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
        public async Task LoadPaymentMethods()
        {
            int ClientID = GetCurrentUserId();
            var response = await _apiService.GetPaymentMethodByID(ClientID);
            if (response.IsSuccess)
            {
                ClientPaymentMethod = response.Data;

            }
        }
        public async Task LoadClientProfile()
        {
            // Get current user ID from your authentication system
            int ClientId = GetCurrentUserId();

            var response = await _apiService.GetClientByID(ClientId);
            if (response.IsSuccess)
            {
                ClientProfile = response.Data;
                LoadPaymentMethodCommand.Execute(null);
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
    }
}
