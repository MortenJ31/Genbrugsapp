
using Core; 

namespace APIServer.Repositories
{
    public interface IAdRepository
    {
        Task<List<Ad>> GetAllAdsAsync();
        Task<Ad?> GetAdByIdAsync(string id); 
        Task AddAdAsync(Ad newAd);
        Task UpdateAdAsync(string id, Ad updatedAd);
        Task DeleteAdAsync(string id);
        Task<List<Ad>> GetFilteredAdsAsync(string? searchQuery, double? minPrice, double? maxPrice, string? categoryId);
        Task<List<Ad>> GetAdsByUserIdAsync(string userId);


    }
}