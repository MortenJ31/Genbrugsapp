using APIServer.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using APIServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Tilføj MongoDB-indstillinger fra appsettings.json
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));


// Registrer MongoClient som singleton, så vi kan bruge den i vores services
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
	// Hent MongoDbSettings og opret en MongoClient
	var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
	return new MongoClient(settings.ConnectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
