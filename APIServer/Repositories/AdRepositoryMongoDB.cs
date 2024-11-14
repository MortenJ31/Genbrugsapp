using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace APIServer.Repositories
{
    public class AdRepositoryMongoDB : IAdRepository
    {
        private readonly IMongoCollection<Ad> _ads;

        public AdRepositoryMongoDB(IMongoDatabase database)
        {
            _ads = database.GetCollection<Ad>("Ad"); 
        }

        public async Task<List<Ad>> GetAllAdsAsync()
        {
            return await _ads.Find(ad => true).ToListAsync();
        }

        public async Task<Ad?> GetAdByIdAsync(string id) // Matcher nu nullable returntype
        {
            return await _ads.Find(ad => ad.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAdAsync(Ad newAd)
        {
            await _ads.InsertOneAsync(newAd);
        }

        public async Task UpdateAdAsync(string id, Ad updatedAd)
        {
            await _ads.ReplaceOneAsync(ad => ad.Id == id, updatedAd);
        }

        public async Task DeleteAdAsync(string id)
        {
            await _ads.DeleteOneAsync(ad => ad.Id == id);
        }
    }
}