using APIServer.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Core;
using Core.Models;
using MongoDB.Bson;

namespace APIServer.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Purchase> _purchases;
        private readonly IMongoCollection<Ad> _ads;


        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _purchases = database.GetCollection<Purchase>("Purchase");
            _ads = database.GetCollection<Ad>("Ad");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("User");
        public IMongoCollection<Ad> Ads => _database.GetCollection<Ad>("Ad");
        public IMongoCollection<Purchase> Purchases => _database.GetCollection<Purchase>("Purchase");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Category");
        public IMongoCollection<Location> Locations => _database.GetCollection<Location>("Location");

        
    }

}




