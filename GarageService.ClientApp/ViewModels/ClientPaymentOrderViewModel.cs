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
    [QueryProperty(nameof(PremiumOfferId), "PremiumOfferId")]
    public class ClientPaymentOrderViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadPaymentMethodCommand { get; }
        
        public ICommand AddPaymentMethodCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }
        public bool IsSelected = false;

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

        private ClientPaymentMethod _selectedPaymentMethod;
        public ClientPaymentMethod SelectedPaymentMethod
        {
            get => _selectedPaymentMethod;
            set => SetProperty(ref _selectedPaymentMethod, value);
        }

        private ClientPaymentOrder _ClientPaymentOrder;
        public ClientPaymentOrder ClientPaymentOrder
        {
            get => _ClientPaymentOrder;
            set
            {
                if (_ClientPaymentOrder != value)
                {
                    _ClientPaymentOrder = value;
                    OnPropertyChanged(nameof(ClientPaymentOrder));
                }
            }
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

        public ClientPaymentOrderViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _apiService = apiservice;
            _sessionService = sessionService;
            LoadPaymentMethodCommand = new Command(async () => await LoadPaymentMethods());
            BackCommand = new Command(async () => await GoBack());
            SaveCommand = new Command(async () => await SavePaymentOrder());
            AddPaymentMethodCommand = new Command(async () => await AddPaymentMethod());
            LoadClientProfile();
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }
        private async Task AddPaymentMethod()
        {
            //await Shell.Current.GoToAsync(nameof(ClientPaymentMethodPage));
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

        public async Task SavePaymentOrder()
        {
            if (SelectedPaymentMethod == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a payment method.", "OK");
                return;
            }
            int ClientID = GetCurrentUserId();
            string OrderStatus = "Pending";
            string OrderNumber = ClientID.ToString() + "-" + DateTime.Now.Ticks.ToString();

            var ClientPaymentOrder = new ClientPaymentOrder
            {
                OrderNumber = OrderNumber,
                ClientId = ClientID,
                Amount = PremiumOffer.PremiumCost,
                Currid = PremiumOffer.CurrId,
                PaymentMethodId = SelectedPaymentMethod.Id,
                Status = OrderStatus,
                CreatedDate = DateTime.Now,
                ProcessedDate = DateTime.Now,
                PremiumOfferid = PremiumOffer.Id
            };

            var response = await _apiService.AddClientPaymentOrder(ClientPaymentOrder);
            if (response.IsSuccess)
            {
                ClientPaymentOrder = response.Data;
                await Shell.Current.GoToAsync($"{nameof(PaymentPage)}?PaymentOrderid={ClientPaymentOrder.Id}");
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
        private PremiumOffer _premiumOffer;
        public PremiumOffer PremiumOffer
        {
            get => _premiumOffer;
            set
            {
                if (_premiumOffer != value)
                {
                    _premiumOffer = value;
                    OnPropertyChanged(nameof(PremiumOffer));
                }
            }
        }

        private int _PremiumOfferId;
        public int PremiumOfferId
        {
            get => _PremiumOfferId;
            set
            {
                _PremiumOfferId = value;
                LoadPremuium();
            }
        }
        public async Task LoadPremuium()
        {
            var response = await _apiService.GetPremiumByID(PremiumOfferId);
            if (response.IsSuccess)
            {
                PremiumOffer = response.Data;
            }
        }
    }
}
