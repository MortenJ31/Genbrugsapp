using APIServer.Repositories;
using Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdController : ControllerBase
    {
        private readonly IAdRepository _adRepository;
        private readonly ILocationRepository _locationRepository;

        public AdController(IAdRepository adRepository, ILocationRepository locationRepository)
        {
            _adRepository = adRepository;
            _locationRepository = locationRepository;
        }

        // GET: api/Ad
        [HttpGet]
        public async Task<IActionResult> GetAllAds()
        {
            var ads = await _adRepository.GetAllAdsAsync();
            return Ok(ads);
        }

        // GET: api/Ad/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdById(string id)
        {
            var ad = await _adRepository.GetAdByIdAsync(id);
            if (ad == null) return NotFound();
            return Ok(ad);
        }

        // POST: api/Ad
        [HttpPost]
        public async Task<IActionResult> CreateAd([FromBody] Ad ad)
        {
            // Simpel tilføjelse af CreatedAt
            ad.CreatedAt = DateTime.UtcNow;

            // Tilføj Location hvis det er angivet
            if (!string.IsNullOrEmpty(ad.LocationAddress))
            {
                var location = await GetOrCreateLocationAsync(ad.LocationAddress);
                ad.LocationId = location.Id;
            }

            await _adRepository.AddAdAsync(ad);
            return CreatedAtAction(nameof(GetAdById), new { id = ad.Id }, ad);
        }

        // PUT: api/Ad/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAd(string id, [FromBody] Ad updatedAd)
        {
            var existingAd = await _adRepository.GetAdByIdAsync(id);
            if (existingAd == null) return NotFound();

            // Kopiér felter fra updatedAd til existingAd
            existingAd.Title = updatedAd.Title;
            existingAd.Description = updatedAd.Description;
            existingAd.Price = updatedAd.Price;
            existingAd.Status = updatedAd.Status;
            existingAd.ImageUrl = updatedAd.ImageUrl;
            existingAd.CategoryId = updatedAd.CategoryId;
            existingAd.LocationId = updatedAd.LocationId;
            existingAd.LocationName = updatedAd.LocationName;
            existingAd.LocationAddress = updatedAd.LocationAddress;

            await _adRepository.UpdateAdAsync(id, existingAd);
            return NoContent();
        }

        // DELETE: api/Ad/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAd(string id)
        {
            var existingAd = await _adRepository.GetAdByIdAsync(id);
            if (existingAd == null) return NotFound();

            await _adRepository.DeleteAdAsync(id);
            return NoContent();
        }

        private async Task<Location> GetOrCreateLocationAsync(string locationAddress)
        {
            var locations = await _locationRepository.GetAllLocationsAsync();
            var existingLocation = locations?.FirstOrDefault(loc => loc.Address == locationAddress);

            if (existingLocation == null)
            {
                var newLocation = new Location { Name = locationAddress, Address = locationAddress };
                await _locationRepository.AddLocationAsync(newLocation);
                return newLocation;
            }

            return existingLocation;
        }
    }
}
