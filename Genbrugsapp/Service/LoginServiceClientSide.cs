using Blazored.LocalStorage;
using Genbrugsapp.Service;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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

    // Implementering af Login-metoden
    public async Task<LoginResponse> Login(string username, string password)
    {
        var loginRequest = new { Username = username, Password = password };
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
        else
        {
            return new LoginResponse
            {
                UserId = null,
                Username = null
            };
        }
    }

    // Implementering af GetUserLoggedIn-metoden
    public async Task<User?> GetUserLoggedIn()
    {
        var userId = await _localStorageService.GetItemAsync<string>("userId");
        var username = await _localStorageService.GetItemAsync<string>("username");

        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(username))
        {
            return new User { UserId = userId, Username = username };
        }

        return null;
    }

    // Implementering af Logout-metoden
    public async Task Logout()
    {
        await _localStorageService.RemoveItemAsync("userId");
        await _localStorageService.RemoveItemAsync("username");
    }

    // Implementering af GetUserId-metoden (ny)
    public async Task<string> GetUserId()
    {
        var userId = await _localStorageService.GetItemAsync<string>("userId");
        return userId;
    }

    // Implementering af SetUserId-metoden (ny)
    public async Task SetUserId(string userId)
    {
        await _localStorageService.SetItemAsync("userId", userId);
    }
}
