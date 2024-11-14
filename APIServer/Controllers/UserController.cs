using APIServer.Repositories;
using APIServer.Services;
using Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MongoDB.Driver;
using Core.Models;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly MongoDbService _mongoDbService;

        public UserController(IUserRepository userRepository, MongoDbService mongoDbService)
        {
            _userRepository = userRepository;
            _mongoDbService = mongoDbService;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                return Conflict("Brugernavnet er allerede i brug.");
            }

            user.CreatedAt = DateTime.UtcNow;
            await _userRepository.CreateUserAsync(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _mongoDbService.Users
                .Find(u => u.Username == loginRequest.Username && u.Password == loginRequest.Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized(); // Returner 401, hvis login fejler
            }

            return Ok(user); // Returner brugeren, hvis login er vellykket
        }

    }
}