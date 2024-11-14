using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace APIServer.Repositories
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllPurchasesAsync(); // Hvis denne metode også er nødvendig
        Task<Purchase> GetPurchaseByIdAsync(string id); // Tilføj denne linje
        Task<List<Purchase>> GetPurchasesByUserIdAsync(string userId); // Hvis denne metode også bruges
        Task CreatePurchaseAsync(Purchase purchase); // Hvis du har behov for at oprette køb
    }
}
