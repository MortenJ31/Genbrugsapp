using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

public class PurchasesRepository
{
    private readonly IMongoCollection<Purchase> _purchases;

    public PurchasesRepository(IOptions<MongoDbSettings> settings, IMongoClient client)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _purchases = database.GetCollection<Purchase>("Purchases");
    }

    public async Task<List<Purchase>> GetSelectedPurchasesAsync()
    {
        // Hent valgte indkøb, som endnu ikke er bekræftet
        return await _purchases.Find(p => !p.IsSelected).ToListAsync();
    }

    public async Task<List<Purchase>> GetConfirmedPurchasesAsync()
    {
        // Hent bekræftede indkøb
        return await _purchases.Find(p => p.IsSelected).ToListAsync();
    }

    public async Task AddPurchaseAsync(Purchase purchase)
    {
        // Tilføj et nyt køb
        await _purchases.InsertOneAsync(purchase);
    }

    public async Task ConfirmPurchasesAsync(List<Purchase> purchases)
    {
        foreach (var purchase in purchases)
        {
            purchase.IsSelected = true; // Marker køb som bekræftet
            await _purchases.ReplaceOneAsync(p => p.Id == purchase.Id, purchase);
        }
    }

    public async Task DeletePurchaseAsync(string id)
    {
        // Slet køb baseret på Id
        await _purchases.DeleteOneAsync(p => p.Id == id);
    }
}