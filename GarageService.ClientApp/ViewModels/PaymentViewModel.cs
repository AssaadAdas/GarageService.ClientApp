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
    [QueryProperty(nameof(PaymentOrderid), "PaymentOrderid")]
    public class PaymentViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadPaymentOrderCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }

        public PaymentViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _apiService = apiservice;
            _sessionService = sessionService;
            LoadPaymentOrderCommand = new Command(async () => await LoadPaymentOrder());
            SaveCommand = new Command(async () => await Save());

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

        private ClientPremiumRegistration _ClientPremiumRegistration;
        public ClientPremiumRegistration ClientPremiumRegistrations
        {
            get => _ClientPremiumRegistration;
            set
            {
                if (_ClientPremiumRegistration != value)
                {
                    _ClientPremiumRegistration = value;
                    OnPropertyChanged(nameof(ClientPremiumRegistrations));
                }
            }
        }

        private int _PaymentOrderid;
        public int PaymentOrderid
        {
            get => _PaymentOrderid;
            set
            {
                _PaymentOrderid = value;
                LoadPaymentOrderCommand.Execute(null);
            }
        }
        private Decimal _Totalamount;
        public Decimal Totalamount
        {
            get => _Totalamount;
            set
            {
                _Totalamount = value;
                OnPropertyChanged(nameof(Totalamount));
            }
        }
        public async Task Save()
        {
            string OrderStatus = "Processed";
            // Update garage Profiles
            ClientPaymentOrder.Status = OrderStatus;

            bool success = await _apiService.UpdateOrderStatusAsync(ClientPaymentOrder.Id, OrderStatus);

            if (success)
            {
                var ClientPremium = new ClientPremiumRegistration
                {
                    Clientid = ClientPaymentOrder.ClientId,
                    Registerdate = DateTime.Now,
                    ExpiryDate = DateTime.Now.AddYears(1), // Assuming 1 year premium
                    IsActive = true
                };

                var response = await _apiService.AddClientPremium(ClientPremium);
                if (response.IsSuccess)
                {
                    ClientPremiumRegistrations = response.Data;
                    var Result = await _apiService.UpdateClientPremiumStatusAsync(ClientPaymentOrder.ClientId, true);
                    if (Result)
                    {
                        await Shell.Current.DisplayAlert("Success", "Order Payed successfully", "OK");
                        await Shell.Current.GoToAsync($"{nameof(ClientDashboardPage)}"); // This pops the Edit page and returns to the dashboard
                    }

                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to register Premium", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to Pay Order", "OK");
            }
        }
        public async Task LoadPaymentOrder()
        {
            // Get current user ID from your authentication system
            var response = await _apiService.GetClientOrderByID(PaymentOrderid);
            if (response.IsSuccess)
            {
                ClientPaymentOrder = response.Data;
                Totalamount = ClientPaymentOrder.Amount;
            }
        }
    }
}
