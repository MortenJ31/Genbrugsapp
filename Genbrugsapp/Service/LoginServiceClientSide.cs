using Blazored.LocalStorage;
using Core; 
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Genbrugsapp.Service
{
    public class LoginServiceClientSide : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        // Konstruktor til at injicere HttpClient og LocalStorageService
        public LoginServiceClientSide(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        // Login-metoden med LoginRequest og LoginResponse fra Core
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/user/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    await _localStorageService.SetItemAsync("userId", result.UserId);
                    await _localStorageService.SetItemAsync("username", result.Username);
                }
                return result;
            }
            return null; // Returner null ved fejl
        }

        // GetUserLoggedIn-metoden for at hente den aktuelle bruger fra LocalStorage
        public async Task<User?> GetUserLoggedIn()
        {
            var userId = await _localStorageService.GetItemAsync<string>("userId");
            var username = await _localStorageService.GetItemAsync<string>("username");

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(username))
            {
                return new User
                {
                    UserId = userId,
                    Username = username
                };
            }
            return null; // Ingen bruger er logget ind
        }

        // Logout-metoden for at fjerne brugerdata fra LocalStorage
        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("userId");
            await _localStorageService.RemoveItemAsync("username");
        }

        // GetUserId-metoden for kun at hente brugerens ID
        public async Task<string> GetUserId()
        {
            return await _localStorageService.GetItemAsync<string>("userId");
        }

        // SetUserId-metoden for at gemme brugerens ID
        public async Task SetUserId(string userId)
        {
            await _localStorageService.SetItemAsync("userId", userId);
        }
    }
}
