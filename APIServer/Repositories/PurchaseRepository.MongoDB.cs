using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using APIServer.Services;

namespace APIServer.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IMongoCollection<Purchase> _purchases;

        public PurchaseRepository(MongoDbService mongoDbService)
        {
            _purchases = mongoDbService.Purchases;
        }

        public async Task<List<Purchase>> GetAllPurchasesAsync()
        {
            return await _purchases.Find(_ => true).ToListAsync();
        }

        // Hent alle køb for en specifik userId
        public async Task<List<Purchase>> GetPurchasesByUserIdAsync(string userId)
        {
            return await _purchases.Find(p => p.UserId == userId).ToListAsync();
        }

        public async Task CreatePurchaseAsync(Purchase purchase)
        {
            await _purchases.InsertOneAsync(purchase);
        }

        public async Task<Purchase> GetPurchaseByIdAsync(string id)
        {
            return await _purchases.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}
