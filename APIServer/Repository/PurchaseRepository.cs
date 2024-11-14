using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using APIServer.Models;
using Core.Models;
using MongoDB.Bson;

namespace APIServer.Repositories
{
    public class PurchasesRepository
    {
        private readonly IMongoCollection<Purchase> _purchases;
        private readonly IMongoCollection<Ad> _ads;

        public PurchasesRepository(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _purchases = database.GetCollection<Purchase>("Purchases");
            _ads = database.GetCollection<Ad>("Ad"); // Til lookup i Ad-kollektionen
        }

        public async Task<List<PurchaseDetail>> GetPurchasesByUserIdAsync(string userId)
        {
            var pipeline = new BsonDocument[]
            {
            new BsonDocument("$match", new BsonDocument("userId", userId)),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "Ad" },
                { "localField", "adId" },
                { "foreignField", "_id" },
                { "as", "adDetails" }
            }),
            new BsonDocument("$unwind", "$adDetails"),
            new BsonDocument("$project", new BsonDocument
            {
                { "purchaseId", "$_id" },
                { "purchaseDate", "$purchaseDate" },
                { "status", "$status" },
                { "locationId", "$locationId" },
                { "adTitle", "$adDetails.title" },
                { "adDescription", "$adDetails.description" },
                { "adPrice", "$adDetails.price" },
                { "adImageUrl", "$adDetails.imageUrl" }
            })
            };

            return await _purchases.Aggregate<PurchaseDetail>(pipeline).ToListAsync();
        }


        public async Task<Purchase> GetPurchaseByIdAsync(string id)
        {
            return await _purchases.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Purchase>> GetSelectedPurchasesAsync()
        {
            return await _purchases.Find(p => !p.IsSelected).ToListAsync();
        }

        public async Task<List<Purchase>> GetConfirmedPurchasesAsync()
        {
            return await _purchases.Find(p => p.IsSelected).ToListAsync();
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _purchases.InsertOneAsync(purchase);
        }

        public async Task ConfirmPurchasesAsync(List<Purchase> purchases)
        {
            foreach (var purchase in purchases)
            {
                purchase.IsSelected = true;
                await _purchases.ReplaceOneAsync(p => p.Id == purchase.Id, purchase);
            }
        }

        public async Task DeletePurchaseAsync(string id)
        {
            await _purchases.DeleteOneAsync(p => p.Id == id);
        }
    }
}

