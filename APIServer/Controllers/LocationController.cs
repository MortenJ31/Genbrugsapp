using APIServer.Repositories;
using Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        // GET: api/Location
        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _locationRepository.GetAllLocationsAsync();
            return Ok(locations);
        }

        // POST: api/Location
        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] Location location)
        {
            location.Id = null;
            await _locationRepository.AddLocationAsync(location);
            return CreatedAtAction(nameof(GetAllLocations), new { id = location.Id }, location);
        }
    }
}