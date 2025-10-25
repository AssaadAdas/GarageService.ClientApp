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
    public class PremiumOffersViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        private readonly ISessionService _sessionService;
        public ICommand LoadPremiumOffersCommand { get; }
        public ICommand LoadProfileCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SelectOfferCommand { get; }

        public ICommand SaveCommand { get; }
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

        private PremiumOffer _selectedPremiumOffer;
        public PremiumOffer SelectedPremiumOffer
        {
            get => _selectedPremiumOffer;
            set => SetProperty(ref _selectedPremiumOffer, value);
        }


        private ObservableCollection<PremiumOffer> _premiumOffers;
        public ObservableCollection<PremiumOffer> PremiumOffers
        {
            get => _premiumOffers;
            set => SetProperty(ref _premiumOffers, value);
        }
        public PremiumOffersViewModel(ApiService apiservice, ISessionService sessionService)
        {
            _ApiService = apiservice;
            _sessionService = sessionService;
            BackCommand = new Command(async () => await GoBack());
            LoadProfileCommand = new Command(async () => await LoadClientProfile());
            SaveCommand = new Command(async () => await SubscribeOrder());
            LoadProfileCommand.Execute(null);
            LoadPremiumOffersCommand = new Command(async () => await LoadPremiumOffers());
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private async Task LoadPremiumOffers()
        {
            // Get current user ID from your authentication system
            var response = await _ApiService.GetOffersByUserType(ClientProfile.User.UserTypeid);
            if (response.IsSuccess)
            {

                PremiumOffers = new ObservableCollection<PremiumOffer>(response.Data);
            }
            else
            {
                // Handle the error, e.g., show a message to the user
                ErrorMessage = response.ErrorMessage;
            }
        }

        private async Task SubscribeOrder()
        {
            if (SelectedPremiumOffer == null)
            {
                await Shell.Current.DisplayAlert("Select Offer", "Please select a premium offer.", "OK");
                return;
            }

            // Navigate to the order page, passing the selected offer's ID
            await Shell.Current.GoToAsync($"{nameof(ClientPaymentOrderPage)}?PremiumOfferId={SelectedPremiumOffer.Id}");
        }

        public async Task LoadClientProfile()
        {
            // Get current user ID from your authentication system
            int ClientId = GetCurrentUserId();

            var response = await _ApiService.GetClientByID(ClientId);
            if (response.IsSuccess)
            {
                ClientProfile = response.Data;
                LoadPremiumOffersCommand.Execute(null);
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
