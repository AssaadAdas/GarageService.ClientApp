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
    public class ClientRegistrationViewModel : BaseViewModel
    {
        private readonly ApiService _ApiService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int UserTypeid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountryId { get; set; }
        public string PhoneExt { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; } = 2;
        public bool IsPremium { get; set; } = false;

        public ICommand RegisterCommand { get; }

        public ClientRegistrationViewModel(ApiService apiservice)
        {
            _ApiService = apiservice;
            RegisterCommand = new Command(async () => await Register());
        }

        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword) != string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }
            //            var usertype =  (UserType) _ApiService.GetUserType(2).Result.Data;   
            var userTypeResponse = await _ApiService.GetUserType(2); // 2 = client user type
            if (!userTypeResponse.IsSuccess || userTypeResponse.Data == null)
            {
                await Shell.Current.DisplayAlert("Error", userTypeResponse.ErrorMessage ?? "Failed to get user type", "OK");
                return;
            }
            var usertype = userTypeResponse.Data;
            // Create user
            var user = new User
            {
                Username = Username,
                Password = Password, // Hash this in production
                UserType = usertype, // Assuming UserTypeid 2 is for clients
                UserTypeid = usertype.Id
            };

            var userAddedResponse = await _ApiService.UserRegister(user);
            if (!userAddedResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create user", "OK");
                return;
            }

            // Get the user with ID


            var userAdded = await _ApiService.GetUserByUsername(Username);
            if (userAdded != null && userAdded.IsSuccess)
            {
                user  = userAdded.Data; // Extract the User object
            }


            // Create client profile
            var clientProfile = new ClientProfile
            {
                FirstName = FirstName,
                LastName = LastName,
                CountryId = 1,
                PhoneExt = "+961",
                PhoneNumber = PhoneNumber,
                Email = Email,
                Address = Address,
                IsPremium =false,
                UserId = user.Id
            };

            var profileAddedResponse = await _ApiService.ClientRegister(clientProfile);

            if (profileAddedResponse.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "Registration successful", "OK");
                await Shell.Current.GoToAsync("//Login");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create client profile", "OK");
            }
        }
    }
}
