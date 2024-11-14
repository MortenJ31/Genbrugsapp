using APIServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using APIServer.Services;
using Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Register MongoClient as a singleton for use in services
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<MongoDbService>();

// Configure CORS to allow specific origins, including local Blazor client and optional dynamic origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:5176", "https://localhost:7251") // Add other allowed URLs here
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    // For development convenience, allow all origins policy
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
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

    // Use more permissive CORS policy in development if needed
    app.UseCors("AllowAllOrigins");
}
else
{
    // Use stricter CORS policy in production
    app.UseCors("AllowSpecificOrigins");
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
