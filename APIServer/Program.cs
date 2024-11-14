using APIServer.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using APIServer.Repositories;
using APIServer.Services;
using Core;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Konfigurer Cookie-baseret autentificering
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Rute til login-side
        options.AccessDeniedPath = "/Account/AccessDenied"; // Rute til adgang nægtet-side
    });

builder.Services.AddAuthorization();

// Tilføj MongoDB-indstillinger fra appsettings.json
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Registrer MongoClient som singleton
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Registrér IMongoDatabase baseret på MongoDbSettings
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return client.GetDatabase(settings.DatabaseName);
});

// Registrer MongoDbService, hvis det bruges andre steder
builder.Services.AddSingleton<MongoDbService>();

// Registrér IAdRepository med AdRepositoryMongoDB som implementering
builder.Services.AddSingleton<IAdRepository, AdRepositoryMongoDB>();
builder.Services.AddSingleton<ILocationRepository, LocationRepositoryMongoDB>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepositoryMongoDB>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();

// Konfigurer CORS til at tillade kun Blazor-klientens URL
builder.Services.AddCors(options =>
{
    options.AddPolicy("policy", policy =>
    {
        policy.WithOrigins("http://localhost:5176") // Porten til din Blazor WebAssembly-app
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Register HttpClient service
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
var app = builder.Build();

// Konfigurer middleware-pipelinen
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("policy"); // Anvend CORS-politikken
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();