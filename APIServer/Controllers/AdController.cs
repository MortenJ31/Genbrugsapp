using APIServer.Services;
using Core;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public AdController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        // GET: api/Ad
        [HttpGet]
        public async Task<IActionResult> GetAllAds()
        {
            var ads = await _mongoDbService.Ads.Find(_ => true).ToListAsync();
            return Ok(ads);
        }

        // GET: api/Ad/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdById(string id)
        {
            var ad = await _mongoDbService.Ads.Find(a => a.Id == id).FirstOrDefaultAsync();
            if (ad == null) return NotFound();
            return Ok(ad);
        }

        // POST: api/Ad
        [HttpPost]
        public async Task<IActionResult> CreateAd([FromBody] Ad ad)
        {
            ad.CreatedAt = DateTime.UtcNow;
            await _mongoDbService.Ads.InsertOneAsync(ad);
            return CreatedAtAction(nameof(GetAdById), new { id = ad.Id }, ad);
        }

        // PUT: api/Ad/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAd(string id, [FromBody] Ad updatedAd)
        {
            var result = await _mongoDbService.Ads.ReplaceOneAsync(a => a.Id == id, updatedAd);
            if (result.MatchedCount == 0) return NotFound();
            return NoContent();
        }

        // DELETE: api/Ad/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAd(string id)
        {
            var result = await _mongoDbService.Ads.DeleteOneAsync(a => a.Id == id);
            if (result.DeletedCount == 0) return NotFound();
            return NoContent();
        }
    }
}
