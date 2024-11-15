using APIServer.Repositories;
using Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

            // Populate each ad with the Location data
            foreach (var ad in ads)
            {
                if (!string.IsNullOrEmpty(ad.LocationId))
                {
                    ad.Location = await _locationRepository.GetLocationByIdAsync(ad.LocationId);
                }
            }

            return Ok(ads);
        }

        // GET: api/Ad/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdById(string id)
        {
            var ad = await _adRepository.GetAdByIdAsync(id);
            if (ad == null) return NotFound();

            // Populate Location if LocationId is set
            if (!string.IsNullOrEmpty(ad.LocationId))
            {
                ad.Location = await _locationRepository.GetLocationByIdAsync(ad.LocationId);
            }

            return Ok(ad);
        }

        // GET: api/Ad/filter
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredAds(
            [FromQuery] string? searchQuery,
            [FromQuery] double? minPrice,
            [FromQuery] double? maxPrice,
            [FromQuery] string? categoryId)
        {
            try
            {
                var ads = await _adRepository.GetFilteredAdsAsync(searchQuery, minPrice, maxPrice, categoryId);

                // Populate each ad with the Location data
                foreach (var ad in ads)
                {
                    if (!string.IsNullOrEmpty(ad.LocationId))
                    {
                        ad.Location = await _locationRepository.GetLocationByIdAsync(ad.LocationId);
                    }
                }

                return Ok(ads);
            }
            catch (Exception ex)
            {
                // Log fejl og returnér en passende fejlmeddelelse
                Console.WriteLine($"Error in GetFilteredAds: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: api/Ad
        [HttpPost]
        public async Task<IActionResult> CreateAd([FromBody] Ad ad)
        {
            ad.CreatedAt = DateTime.UtcNow;

            // Assign LocationId if a location address is provided
            if (ad.Location != null && !string.IsNullOrEmpty(ad.Location.Address))
            {
                var location = await GetOrCreateLocationAsync(ad.Location);
                ad.LocationId = location.Id;
                ad.Location = location; // Populate the ad's Location property
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

            // Copy fields from updatedAd to existingAd
            existingAd.Title = updatedAd.Title;
            existingAd.Description = updatedAd.Description;
            existingAd.Price = updatedAd.Price;
            existingAd.Status = updatedAd.Status;
            existingAd.ImageUrl = updatedAd.ImageUrl;
            existingAd.CategoryId = updatedAd.CategoryId;

            // Handle Location
            if (updatedAd.Location != null && !string.IsNullOrEmpty(updatedAd.Location.Address))
            {
                var location = await GetOrCreateLocationAsync(updatedAd.Location);
                existingAd.LocationId = location.Id;
                existingAd.Location = location; // Update in-memory Location reference
            }

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

        // Helper function to get or create a location
        private async Task<Location> GetOrCreateLocationAsync(Location location)
        {
            // Find if a location with the same address exists
            var locations = await _locationRepository.GetAllLocationsAsync();
            var existingLocation = locations?.FirstOrDefault(loc => loc.Address == location.Address);

            if (existingLocation == null)
            {
                // Create a new location if it doesn't exist
                var newLocation = new Location { Classroom = location.Classroom, Address = location.Address };
                await _locationRepository.AddLocationAsync(newLocation);
                return newLocation;
            }

            return existingLocation;
        }
    }
}
