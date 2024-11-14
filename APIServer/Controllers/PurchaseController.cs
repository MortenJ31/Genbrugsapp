using APIServer.Services;
using Core;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Core.Models;
using APIServer.Repositories;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchasesRepository _purchasesRepository;

        public PurchaseController(PurchasesRepository purchasesRepository)
        {
            _purchasesRepository = purchasesRepository;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPurchasesByUserId(string userId)
        {
            var purchaseDetails = await _purchasesRepository.GetPurchasesByUserIdAsync(userId);
            if (purchaseDetails == null || !purchaseDetails.Any())
            {
                return NotFound("Ingen køb fundet for brugeren.");
            }
            return Ok(purchaseDetails);
        }

        // GET: api/Purchase?userId={userId}
        [HttpGet]
        public async Task<IActionResult> GetPurchasesByUserIdQuery([FromQuery] string userId)
        {
            // Konverter userId til ObjectId, hvis nødvendigt
            var purchases = await _mongoDbService.Purchases
                .Find(p => p.UserId == userId)
                .ToListAsync();
            return Ok(purchases);
        }

        // GET: api/Purchase/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseById(string id)
        {
            var purchase = await _purchasesRepository.GetPurchaseByIdAsync(id);
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

