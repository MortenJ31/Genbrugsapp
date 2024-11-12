using APIServer.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace APIServer.Services
{
	public class MongoDbService
	{
		private readonly IMongoDatabase _database;

		public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings, IMongoClient mongoClient)
		{
			_database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
		}

		// Eksempel på at få en User Collection
		public IMongoCollection<User> Users => _database.GetCollection<User>("User");
	}
}
