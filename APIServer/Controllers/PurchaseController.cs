using APIServer.Services;
using Core;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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

        // GET: api/Purchase
        [HttpGet]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _mongoDbService.Purchases.Find(_ => true).ToListAsync();
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
            purchase.Status = "completed"; // Eksempelstatus, kan ændres til "pending" afhængigt af forretningslogik
            await _mongoDbService.Purchases.InsertOneAsync(purchase);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
        }
    }
}
