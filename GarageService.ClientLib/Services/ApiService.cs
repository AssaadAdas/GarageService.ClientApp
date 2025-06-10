using GarageService.ClientLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GarageService.ClientLib.Services
{
    public interface IApiService
    {
        Task<ApiResponse<User>> UserRegister(User user);
        Task<ApiResponse<ClientProfile>> ClientRegister(ClientProfile Client);
        Task<ApiResponse<User>> GetUserByUsername(string username);
    }

    public class ApiService 
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://localhost:44319/api/";

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _httpClient.Timeout = TimeSpan.FromSeconds(120);

            // Add any default headers if needed
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // Add other methods (Put, Delete, etc.) as needed

        public async Task<ApiResponse<User>> UserRegister(User user)
        {
            try
            {
                // Call the API register endpoint
                var response = await _httpClient.PostAsJsonAsync("register", user);

                if (response.IsSuccessStatusCode)
                {
                    var registeredUser = await response.Content.ReadFromJsonAsync<User>();


                    return new ApiResponse<User> { Data = registeredUser, IsSuccess = true };
                }
                else
                {
                    // Handle different error status codes
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    return response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.BadRequest =>
                            new ApiResponse<User> { ErrorMessage = errorMessage ?? "Invalid request data", IsSuccess = false },
                        System.Net.HttpStatusCode.Conflict =>
                            new ApiResponse<User> { ErrorMessage = errorMessage ?? "Username already exists", IsSuccess = false },
                        _ =>
                            new ApiResponse<User> { ErrorMessage = $"Registration failed: {response.ReasonPhrase}", IsSuccess = false }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>
                {
                    ErrorMessage = $"An error occurred during registration: {ex.Message}",
                    IsSuccess = false
                };
            }
        }

        public async Task<ApiResponse<ClientProfile>> ClientRegister(ClientProfile Client)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("PostClientProfile", Client);
                if (response.IsSuccessStatusCode)
                {
                    var registeredClient = await response.Content.ReadFromJsonAsync<ClientProfile>();


                    return new ApiResponse<ClientProfile> { Data = registeredClient, IsSuccess = true };
                }
                else
                {
                    // Handle different error status codes
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    return response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.BadRequest =>
                            new ApiResponse<ClientProfile> { ErrorMessage = errorMessage ?? "Invalid request data", IsSuccess = false },
                        System.Net.HttpStatusCode.Conflict =>
                            new ApiResponse<ClientProfile> { ErrorMessage = errorMessage ?? "Username already exists", IsSuccess = false },
                        _ =>
                            new ApiResponse<ClientProfile> { ErrorMessage = $"Registration failed: {response.ReasonPhrase}", IsSuccess = false }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClientProfile>
                {
                    ErrorMessage = $"An error occurred during registration: {ex.Message}",
                    IsSuccess = false
                };
            }
        }
        public async Task<ApiResponse<User>> GetUserByUsername(string username)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("GetUserIdByUserName", username);
                if (response.IsSuccessStatusCode)
                {
                    var ExistUser = await response.Content.ReadFromJsonAsync<User>();


                    return new ApiResponse<User> { Data = ExistUser, IsSuccess = true };
                }
                else
                {
                    // Handle different error status codes
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.BadRequest =>
                            new ApiResponse<User> { ErrorMessage = errorMessage ?? "Invalid request data", IsSuccess = false },
                        System.Net.HttpStatusCode.NotFound =>
                            new ApiResponse<User> { ErrorMessage = errorMessage ?? "User not found", IsSuccess = false },
                        _ =>
                            new ApiResponse<User> { ErrorMessage = $"Check failed: {response.ReasonPhrase}", IsSuccess = false }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>
                {
                    ErrorMessage = $"An error occurred during registration: {ex.Message}",
                    IsSuccess = false
                };
            }
        }
       
        public async Task<ApiResponse<UserType>> GetUserType(int Typeid)
        {
            try
            {
                // Call the API endpoint
          
                using var response = await _httpClient.GetAsync($"UserTypes/{Typeid}");

                if (response.IsSuccessStatusCode)
                {
                    var userType = await response.Content.ReadFromJsonAsync<UserType>();
                    return new ApiResponse<UserType> { Data = userType, IsSuccess = true };
                }

                // Handle non-success status codes
                var errorMessage = await response.Content.ReadAsStringAsync();
                return response.StatusCode switch
                {
                    HttpStatusCode.NotFound =>
                        new ApiResponse<UserType> { ErrorMessage = "User type not found", IsSuccess = false },
                    _ =>
                        new ApiResponse<UserType> { ErrorMessage = $"Error fetching user type: {response.ReasonPhrase}", IsSuccess = false }
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions (network issues, etc.)
                return new ApiResponse<UserType>
                {
                    ErrorMessage = $"An error occurred while fetching user type: {ex.Message}",
                    IsSuccess = false
                };
            }
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
