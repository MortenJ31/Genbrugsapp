using APIServer.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using APIServer.Repositories;
using APIServer.Services;
using Core;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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

// Registrer MongoDbService
builder.Services.AddSingleton<MongoDbService>();

// Registrer Repositories
builder.Services.AddSingleton<IAdRepository, AdRepositoryMongoDB>();
builder.Services.AddSingleton<ILocationRepository, LocationRepositoryMongoDB>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepositoryMongoDB>();

// CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("policy", policy =>
    {
        policy.WithOrigins("http://localhost:5176", "https://localhost:5176")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Configure API and Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("policy");
app.UseAuthorization();

app.MapControllers();

app.Run();
