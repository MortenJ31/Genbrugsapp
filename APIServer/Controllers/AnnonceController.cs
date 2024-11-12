using Microsoft.AspNetCore.Mvc;
using Core;
using APIServer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnonceController : ControllerBase
    {
        private readonly IAnnonceRepository _repository;

        public AnnonceController(IAnnonceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnoncer()
        {
            var annoncer = await _repository.GetAnnoncerAsync();
            return Ok(annoncer);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnnonce([FromBody] Annonce annonce)
        {
            if (annonce == null)
            {
                return BadRequest("Annonce data is null.");
            }

            await _repository.AddAnnonceAsync(annonce);
            return CreatedAtAction(nameof(GetAnnoncer), new { id = annonce.Id }, annonce);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string newStatus)
        {
            if (string.IsNullOrEmpty(newStatus))
            {
                return BadRequest("Status cannot be null or empty.");
            }

            var result = await _repository.UpdateAnnonceStatusAsync(id, newStatus);

            if (!result)
            {
                return NotFound($"Annonce with id '{id}' not found.");
            }

            return NoContent();
        }
    }
}