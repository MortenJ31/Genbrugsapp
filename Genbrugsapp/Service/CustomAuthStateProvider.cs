using Blazored.LocalStorage;
using Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Genbrugsapp.Service
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ILoginService _loginService;

        public CustomAuthStateProvider(ILocalStorageService localStorage, ILoginService loginService)
        {
            _localStorage = localStorage;
            _loginService = loginService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await _loginService.GetUserLoggedIn();

            if (user != null)
            {
                // Håndterer null værdi for Username, tildel standardværdi hvis nødvendigt
                var username = user.Username ?? "Unknown";

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                }, "auth");

                var principal = new ClaimsPrincipal(identity);
                return new AuthenticationState(principal);
            }
            else
            {
                // Ingen bruger logget ind, returner som anonym bruger
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void MarkUserAsAuthenticated(User user)
        {
            // Håndterer null værdi for Username, tildel standardværdi hvis nødvendigt
            var username = user.Username ?? "Unknown";
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
            }, "auth");

            var principal = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public void MarkUserAsLoggedOut()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }
    }
}
