using APIServer.Services;
using Core;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Core.Models;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public PurchaseController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPurchasesByUserId(string userId)
        {
            // Hent køb for den specifikke bruger
            var userPurchases = await _mongoDbService.Purchases
                .Find(p => p.UserId == userId)
                .ToListAsync();

            // Liste til at holde PurchaseDetail-objekter med tilhørende Ad-oplysninger
            var purchaseDetails = new List<PurchaseDetail>();

            foreach (var purchase in userPurchases)
            {
                // Hent Ad-oplysninger baseret på adId fra købet
                var ad = await _mongoDbService.Ads
                    .Find(a => a.Id == purchase.AdId)
                    .FirstOrDefaultAsync();

                if (ad != null)
                {
                    Console.WriteLine($"Ad found with Title: {ad.Title}, Price: {ad.Price}, ImageUrl: {ad.ImageUrl}");

                    purchaseDetails.Add(new PurchaseDetail
                    {
                        Id = purchase.Id,
                        PurchaseDate = purchase.PurchaseDate,
                        Status = purchase.Status,
                        LocationId = purchase.LocationId,
                        Title = ad.Title,
                        Description = ad.Description,
                        Price = ad.Price,
                        ImageUrl = ad.ImageUrl
                    });
                }
                else
                {
                    Console.WriteLine("Ad not found for purchase.");
                }
             }

            return Ok(purchaseDetails);
        }



        // GET: api/Purchase?userId={userId}
        [HttpGet]
        public async Task<IActionResult> GetUserPurchases([FromQuery] string userId)
        {
            // Henter kun de indkøb, der tilhører den angivne bruger
            var purchases = await _mongoDbService.Purchases
                .Find(p => p.UserId == userId)
                .ToListAsync();
            return Ok(purchases);
        }

        // GET: api/Purchase/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseById(string id)
        {
            var purchase = await _mongoDbService.Purchases.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (purchase == null) return NotFound();
            return Ok(purchase);
        }

        // POST: api/Purchase
        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody] Purchase purchase)
        {
            purchase.PurchaseDate = DateTime.UtcNow;
            purchase.Status = "pending"; // Sæt startstatus for nye indkøb
            await _mongoDbService.Purchases.InsertOneAsync(purchase);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
        }

        // PUT: api/Purchase/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchase(string id, [FromBody] Purchase updatedPurchase)
        {
            var result = await _mongoDbService.Purchases.ReplaceOneAsync(p => p.Id == id, updatedPurchase);
            if (result.MatchedCount == 0) return NotFound();
            return NoContent();
        }

        // DELETE: api/Purchase/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(string id)
        {
            var result = await _mongoDbService.Purchases.DeleteOneAsync(p => p.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return NoContent();
        }

        // PATCH: api/Purchase/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePurchaseStatus(string id, [FromBody] string status)
        {
            var update = Builders<Purchase>.Update.Set(p => p.Status, status);
            var result = await _mongoDbService.Purchases.UpdateOneAsync(p => p.Id == id, update);
            if (result.MatchedCount == 0) return NotFound();
            return NoContent();
        }
    }
}

