using APIServer.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using APIServer.Services;
using Core;
using APIServer.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Tilføj MongoDB-indstillinger fra appsettings.json
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Registrer MongoClient som singleton, så vi kan bruge den i vores services
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

// Konfigurer CORS til at tillade kun Blazor-klientens URL
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7251") // Porten til din Blazor WebAssembly-app
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Register HttpClient service
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorClient"); // Anvend CORS-politikken

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();