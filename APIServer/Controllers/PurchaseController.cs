using APIServer.Repositories;
using APIServer.Services;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MongoDB.Driver;

namespace APIServer.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseController(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        // GET: api/Purchase/mineindkob
        [HttpGet("mineindkob")]
        public async Task<IActionResult> GetMyPurchases()
        {
            // Hent brugerens userId fra ClaimsPrincipal
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Bruger-ID ikke fundet.");
            }

            // Hent alle køb for den pågældende userId
            var purchases = await _purchaseRepository.GetPurchasesByUserIdAsync(userId);
            return Ok(purchases);
        }

        // GET: api/Purchase/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseById(string id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            return Ok(purchase);
        }

        // POST: api/Purchase
        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody] Purchase purchase)
        {
            purchase.PurchaseDate = DateTime.UtcNow;
            purchase.Status = "completed"; // Example status, can be changed based on business logic
            await _purchaseRepository.CreatePurchaseAsync(purchase);
            return CreatedAtAction(nameof(GetPurchaseById), new { id = purchase.Id }, purchase);
        }
    }
}
