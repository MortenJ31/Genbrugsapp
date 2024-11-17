using Genbrugsapp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Genbrugsapp.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient for backend API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5066") }); // Backend API

// Services for authorization and local storage
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage(); // LocalStorage service

// Register LoginService (client-side)
builder.Services.AddScoped<ILoginService, LoginServiceClientSide>();
// Register custom AuthenticationStateProvider (if needed)
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();