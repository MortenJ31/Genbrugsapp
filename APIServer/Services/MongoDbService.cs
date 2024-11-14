using APIServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Core;

namespace APIServer.Services
{
	public class MongoDbService
	{
		private readonly IMongoDatabase _database;

		public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings, IMongoClient mongoClient)
		{
			_database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
		}

		public IMongoCollection<User> Users => _database.GetCollection<User>("User");
		public IMongoCollection<Ad> Ads => _database.GetCollection<Ad>("Ad");
		public IMongoCollection<Purchase> Purchases => _database.GetCollection<Purchase>("Purchase");
		public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Category");
		public IMongoCollection<Location> Locations => _database.GetCollection<Location>("Location");
	}
}

