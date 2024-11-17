using MongoDB.Driver;
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

        public async Task<Ad?> GetAdByIdAsync(string id)
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

        public async Task<List<Ad>> GetFilteredAdsAsync(string? searchQuery, double? minPrice, double? maxPrice, string? categoryId)
        {
            var filterBuilder = Builders<Ad>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                filter &= filterBuilder.Regex("title", new MongoDB.Bson.BsonRegularExpression($"^{searchQuery}", "i"));
            }

            if (minPrice.HasValue)
            {
                filter &= filterBuilder.Gte("price", minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                filter &= filterBuilder.Lte("price", maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(categoryId))
            {
                var categoryObjectId = new MongoDB.Bson.ObjectId(categoryId);
                filter &= filterBuilder.Eq("categoryId", categoryObjectId);
            }

            return await _ads.Find(filter).ToListAsync();
        }

        public async Task<List<Ad>> GetAdsByUserIdAsync(string userId)
        {
            var filter = Builders<Ad>.Filter.Eq(ad => ad.UserId, userId); // Find ads by userId
            return await _ads.Find(filter).ToListAsync();
        }
    }
}
