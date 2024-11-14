using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using Core;
using System.Net.Http;
using System.Net.Http.Json;

namespace Genbrugsapp.Service
{
    public class LoginServiceClientSide : ILoginService
    {

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public LoginServiceClientSide(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }


        public async Task<User?> GetUserLoggedIn()
        {
            var res = await _localStorage.GetItemAsync<User>("user");
            return res;
        }
        public async Task<bool> Login(string username, string password)
        {
            // Send login request to the API
            var loginRequest = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7074/api/User/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                // Retrieve user data from the response
                var user = await response.Content.ReadFromJsonAsync<User>();

                // Store the user in local storage
                await _localStorage.SetItemAsync("user", user);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RegisterUser(User newUser)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7074/api/User/register", newUser);
            return response.IsSuccessStatusCode;
        }
    }
}
