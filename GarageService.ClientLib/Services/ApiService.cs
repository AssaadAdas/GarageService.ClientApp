
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
        private const string BaseUrl = "https://localhost:44344/api/";

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
        /// <summary>
        /// Register users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, User RegisteredUser)> RegisterUserAsync(User user)
        {
            try
            {
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Users/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var registeredUser = JsonSerializer.Deserialize<User>(responseContent);
                    return (true, "Registration successful", registeredUser);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return (false, "Username already exists", null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage, null);
                }
                else
                {
                    return (false, $"Registration failed: {response.ReasonPhrase}", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }
     
        /// <summary>
        /// Registers a client profile.
        /// </summary>
        /// <param name="Client"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, ClientProfile RegisteredUser)> ClientRegister(ClientProfile Client)
        {
            try
            {
                var json = JsonSerializer.Serialize(Client);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("ClientProfiles", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var registeredClient = JsonSerializer.Deserialize<ClientProfile>(responseContent);
                    return (true, "Registration successful", registeredClient);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return (false, "Client already exists", null);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage, null);
                }
                else
                {
                    return (false, $"Registration failed: {response.ReasonPhrase}", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Get Offers By User Type
        /// </summary>
        /// <param name="userTypeId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<PremiumOffer>>> GetOffersByUserType(int userTypeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"PremiumOffers/type/{userTypeId}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var premiumoffer = await response.Content.ReadFromJsonAsync<List<PremiumOffer>>();

                    if (premiumoffer == null) // Handle potential null reference
                    {
                        return new ApiResponse<List<PremiumOffer>>
                        {
                            IsSuccess = false,
                            ErrorMessage = "premiumoffer not found"
                        };
                    }

                    //return new ApiResponse<PremiumOffer> { Data = premiumoffer, IsSuccess = true };
                    return new ApiResponse<List<PremiumOffer>> { Data = premiumoffer, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<List<PremiumOffer>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PremiumOffer>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        /// Adds a new vehicle asynchronously.
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, Vehicle vehicle)> AddVehicleAsync(Vehicle vehicle)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicle);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Vehicles", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var AddedVehicle = JsonSerializer.Deserialize<Vehicle>(responseContent);
                    return (true, "Registration successful", AddedVehicle);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }
       

        /// <summary>
        /// get user by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ApiResponse<User>> GetUserByUsername(string username)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Users/GetUserByUserName/{username}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as User
                    var user = await response.Content.ReadFromJsonAsync<User>();

                    if (user == null) // Handle potential null reference
                    {
                        return new ApiResponse<User>
                        {
                            IsSuccess = false,
                            ErrorMessage = "User not found"
                        };
                    }

                    return new ApiResponse<User> { Data = user, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<User>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetches a user type by its ID.
        /// </summary>
        /// <param name="Typeid"></param>
        /// <returns></returns>
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

        /// <summary>
        /// login async
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                var json = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Users/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return JsonSerializer.Deserialize<LoginResponse>(responseContent, options);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Invalid username or password");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Login failed: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                throw new Exception($"Login error: {ex.Message}");
            }
        }
        
        /// <summary>
        /// get client profile by user ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ClientProfile>> GetClientByUserID(int userid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ClientProfiles/GetClientProfileByUserID/{userid}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var clientprofile = await response.Content.ReadFromJsonAsync<ClientProfile>();

                    if (clientprofile == null) // Handle potential null reference
                    {
                        return new ApiResponse<ClientProfile>
                        {
                            IsSuccess = false,
                            ErrorMessage = "clientprofile not found"
                        };
                    }

                    return new ApiResponse<ClientProfile> { Data = clientprofile, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<ClientProfile>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClientProfile>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get client by by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ClientProfile>> GetClientByID(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ClientProfiles/{id}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var clientprofile = await response.Content.ReadFromJsonAsync<ClientProfile>();

                    if (clientprofile == null) // Handle potential null reference
                    {
                        return new ApiResponse<ClientProfile>
                        {
                            IsSuccess = false,
                            ErrorMessage = "clientprofile not found"
                        };
                    }

                    return new ApiResponse<ClientProfile> { Data = clientprofile, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<ClientProfile>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClientProfile>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all countries as list
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Country>>> GetCountriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Countries");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var countries = await response.Content.ReadFromJsonAsync<List<Country>>();
                    //var countries = JsonSerializer.Deserialize<List<Country>>(content);
                    return new ApiResponse<List<Country>>
                    {
                        IsSuccess = true,
                        Data = countries.OrderBy(c => c.CountryName).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<Country>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Country>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// GetManufacturersAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Manufacturer>>> GetManufacturersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Manufacturers");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var manufacturers = await response.Content.ReadFromJsonAsync<List<Manufacturer>>();
                    return new ApiResponse<List<Manufacturer>>
                    {
                        IsSuccess = true,
                        Data = manufacturers.OrderBy(c => c.ManufacturerDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<Manufacturer>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Manufacturer>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// GetMeassureUnitsAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<MeassureUnit>>> GetMeassureUnitsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("MeassureUnits");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var MeassureUnits = await response.Content.ReadFromJsonAsync<List<MeassureUnit>>();
                    return new ApiResponse<List<MeassureUnit>>
                    {
                        IsSuccess = true,
                        Data = MeassureUnits.OrderBy(c => c.MeassureUnitDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<MeassureUnit>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<MeassureUnit>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }


        /// <summary>
        /// GetFuelTypesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<FuelType>>> GetFuelTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("FuelTypes");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var manufacturers = await response.Content.ReadFromJsonAsync<List<FuelType>>();
                    return new ApiResponse<List<FuelType>>
                    {
                        IsSuccess = true,
                        Data = manufacturers.OrderBy(c => c.FuelTypeDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<FuelType>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<FuelType>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// GetVehicleTypesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<VehicleType>>> GetVehicleTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("VehicleTypes");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var vehicletypes = await response.Content.ReadFromJsonAsync<List<VehicleType>>();
                    return new ApiResponse<List<VehicleType>>
                    {
                        IsSuccess = true,
                        Data = vehicletypes.OrderBy(c => c.VehicleTypesDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<VehicleType>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",

                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<VehicleType>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,

                };
            }
        }

        /// <summary>
        /// UpdateClientProfileAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientProfile"></param>
        /// <returns></returns>
        public async Task<bool> UpdateClientProfileAsync(int id, ClientProfile clientProfile)
        {
            try
            {
                var json = JsonSerializer.Serialize(clientProfile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"ClientProfiles/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                   
                    return true;
                }

                // Handle specific status codes if needed
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Client profile not found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("Invalid request - ID mismatch");
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                Console.WriteLine($"Error updating client profile: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// UpdateVehicleAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public async Task<bool> UpdateVehicleAsync(int id, Vehicle vehicle)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicle);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"Vehicles/{id}", content);

                if (response.IsSuccessStatusCode)
                {

                    return true;
                }

                // Handle specific status codes if needed
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("vehicle not found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("Invalid request - ID mismatch");
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                Console.WriteLine($"Error updating vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// GetClientNotification
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ClientNotification>> GetClientNotification(int Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ClientNotifications/{Id}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var clientnotification = await response.Content.ReadFromJsonAsync<ClientNotification>();

                    if (clientnotification == null) // Handle potential null reference
                    {
                        return new ApiResponse<ClientNotification>
                        {
                            IsSuccess = false,
                            ErrorMessage = "ClientNotification not found"
                        };
                    }

                    return new ApiResponse<ClientNotification> { Data = clientnotification, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<ClientNotification>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ClientNotification>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ApiResponse<List<ClientNotification>>> GetClientUnReadNotification(int ClientId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ClientNotifications/UnReadByClient/{ClientId}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var clientnotification = await response.Content.ReadFromJsonAsync<List<ClientNotification>>();

                    if (clientnotification == null) // Handle potential null reference
                    {
                        return new ApiResponse<List<ClientNotification>>
                        {
                            IsSuccess = false,
                            ErrorMessage = "ClientNotification not found"
                        };
                    }

                    return new ApiResponse<List<ClientNotification>> { Data = clientnotification, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<List<ClientNotification>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ClientNotification>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        /// <summary>
        /// UpdateClientNotificationAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clientNotification"></param>
        /// <returns></returns>
        public async Task<bool> UpdateClientNotificationAsync(int id, ClientNotification clientNotification)
        {
            try
            {
                var json = JsonSerializer.Serialize(clientNotification);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"ClientNotifications/{id}", content);

                if (response.IsSuccessStatusCode)
                {

                    return true;
                }

                // Handle specific status codes if needed
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Client profile not found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("Invalid request - ID mismatch");
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                Console.WriteLine($"Error updating client profile: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// GetGaragesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<GarageProfile>>> GetGaragesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("GarageProfiles");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var garage = await response.Content.ReadFromJsonAsync<List<GarageProfile>>();
                    return new ApiResponse<List<GarageProfile>>
                    {
                        IsSuccess = true,
                        Data = garage.OrderBy(c => c.GarageName).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<GarageProfile>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<GarageProfile>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        /// <summary>
        /// GetServiceTypesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<ServiceType>>> GetServiceTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("ServiceTypes");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var serviceTypes = await response.Content.ReadFromJsonAsync<List<ServiceType>>();
                    return new ApiResponse<List<ServiceType>>
                    {
                        IsSuccess = true,
                        Data = serviceTypes.OrderBy(c => c.Description).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<ServiceType>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ServiceType>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        /// <summary>
        /// GetCurremciesAsync
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<List<Currency>>> GetCurremciesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Currencies");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var Currencies = await response.Content.ReadFromJsonAsync<List<Currency>>();
                    return new ApiResponse<List<Currency>>
                    {
                        IsSuccess = true,
                        Data = Currencies.OrderBy(c => c.CurrDesc).ToList(),
                    };
                }
                else
                {
                    return new ApiResponse<List<Currency>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} - {content}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Currency>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
        
        /// <summary>
        /// GetVehicleByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<Vehicle>> GetVehicleByID(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Vehicles/{id}");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as clientprofile
                    var vehicle = await response.Content.ReadFromJsonAsync<Vehicle>();

                    if (vehicle == null) // Handle potential null reference
                    {
                        return new ApiResponse<Vehicle>
                        {
                            IsSuccess = false,
                            ErrorMessage = "Vehicle not found"
                        };
                    }

                    return new ApiResponse<Vehicle> { Data = vehicle, IsSuccess = true };
                }
                else
                {
                    return new ApiResponse<Vehicle>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<Vehicle>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// AddVehiclesServicesAsync
        /// </summary>
        /// <param name="vehicleservice"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehiclesService>> AddVehiclesServicesAsync(VehiclesService vehicleservice)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleservice);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("VehiclesServices", content);
                if (response.IsSuccessStatusCode)
                {
                    
                    var VehicleServices = await response.Content.ReadFromJsonAsync<VehiclesService>();
                    return new ApiResponse<VehiclesService>
                    {
                        IsSuccess = true,
                        Data = VehicleServices,
                    };
                    
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// AddVehiclesRefuleAsync
        /// </summary>
        /// <param name="vehicleRefuel"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehiclesRefuel>> AddVehiclesRefuleAsync(VehiclesRefuel vehicleRefuel)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleRefuel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("VehiclesRefuels", content);
                if (response.IsSuccessStatusCode)
                {

                    var Vehiclerefule = await response.Content.ReadFromJsonAsync<VehiclesRefuel>();
                    return new ApiResponse<VehiclesRefuel>
                    {
                        IsSuccess = true,
                        Data = Vehiclerefule,
                    };

                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// AddVehicleAppointmentAsync
        /// </summary>
        /// <param name="vehicleAppointment"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehicleAppointment>> AddVehicleAppointmentAsync(VehicleAppointment vehicleAppointment)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleAppointment);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("VehicleAppointments", content);
                if (response.IsSuccessStatusCode)
                {

                    var VehicleAppointment = await response.Content.ReadFromJsonAsync<VehicleAppointment>();
                    return new ApiResponse<VehicleAppointment>
                    {
                        IsSuccess = true,
                        Data = VehicleAppointment,
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// AddVehiclesServiceTypeAsync
        /// </summary>
        /// <param name="vehicleservicetype"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message, VehiclesServiceType vehiclesservicetype)> AddVehiclesServiceTypeAsync(VehiclesServiceType vehicleservicetype)
        {
            try
            {
                var json = JsonSerializer.Serialize(vehicleservicetype);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("VehiclesServiceTypes", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var AddedVehicleServicesTypes = JsonSerializer.Deserialize<VehiclesServiceType>(responseContent);
                    return (true, "Registration successful", AddedVehicleServicesTypes);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// display history for vehicles
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns>VehicleHistoryResponse</returns>
        public async Task<VehicleHistoryResponse> GetVehicleHistory(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Vehicles/GetVehicleHistory/{vehicleId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<VehicleHistoryResponse>();
                    return content;
                }

                // Handle non-success status codes
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                // Handle exceptions (network issues, etc.)
                Console.WriteLine($"Error fetching vehicle history: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// AddServicesTypeSetUpAsync
        /// </summary>
        /// <param name="ServicesTypeSetUp"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ServicesTypeSetUp>> AddServicesTypeSetUpAsync(ServicesTypeSetUp ServicesTypeSetUp)
        {
            try
            {
                var json = JsonSerializer.Serialize(ServicesTypeSetUp);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("ServicesTypeSetUps", content);
                if (response.IsSuccessStatusCode)
                {

                    var servicesTypeSetUp = await response.Content.ReadFromJsonAsync<ServicesTypeSetUp>();
                    return new ApiResponse<ServicesTypeSetUp>
                    {
                        IsSuccess = true,
                        Data = servicesTypeSetUp,
                    };

                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();

                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new Exception($"Validation error: {errorContent}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        throw new Exception($"Conflict: {errorContent}");
                    }
                    else
                    {
                        throw new Exception($"API error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error adding serviceType setup: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateServicesTypeSetUpAsync(int id, ServicesTypeSetUp serviceSetUp)
        {
            try
            {
                var json = JsonSerializer.Serialize(serviceSetUp);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"ServicesTypeSetUps/{id}", content);

                if (response.IsSuccessStatusCode)
                {

                    return true;
                }

                // Handle specific status codes if needed
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("ServicesTypeSetUps not found");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("Invalid request - ID mismatch");
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                Console.WriteLine($"Error updating ServicesTypeSetUps: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// GetServicesTypeSetUpVehicleAsync
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ServicesTypeSetUp>>>  GetServicesTypeSetUpVehicleAsync(int vehicleId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"ServicesTypeSetUps/Vehicle/{vehicleId}");

                if (response.IsSuccessStatusCode)
                {
                    var serviceTypes = await response.Content.ReadFromJsonAsync<List<ServicesTypeSetUp>>();
                    return new ApiResponse<List<ServicesTypeSetUp>>
                    {
                        IsSuccess = true,
                        Data = serviceTypes

                    };
                }
                else
                {
                    return new ApiResponse<List<ServicesTypeSetUp>>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Error: {response.StatusCode} ",
                    };
                }

                // Handle non-success status codes
                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                // Handle exceptions (network issues, etc.)
                Console.WriteLine($"Error fetching vehicle history: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> GetVehicleServicesHistory(int vehicleId, int currentOdometer)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Vehicles/GetVehicleServicesHistory/{vehicleId}?CurrentOdometer={currentOdometer}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return bool.Parse(result);
                }
                else
                {
                    // Handle different status codes
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("Vehicle not found");
                    }
                    else
                    {
                        throw new Exception($"API call failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (network issues, etc.)
                throw new Exception($"Error calling API: {ex.Message}");
            }
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public User User { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
