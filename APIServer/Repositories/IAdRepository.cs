using System.Collections.Generic;
using System.Threading.Tasks;
using Core; 

namespace APIServer.Repositories
{
    public interface IAdRepository
    {
        Task<List<Ad>> GetAllAdsAsync();
        Task<Ad?> GetAdByIdAsync(string id); // Ændret til nullable Ad
        Task AddAdAsync(Ad newAd);
        Task UpdateAdAsync(string id, Ad updatedAd);
        Task DeleteAdAsync(string id);
    }
}