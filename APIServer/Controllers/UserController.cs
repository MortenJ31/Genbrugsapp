using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Core;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _users;

        public UserController(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("Users");
        }

        // Endpoint til at registrere brugere
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            // Tjek om brugeren allerede eksisterer
            var existingUser = await _users.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest("Brugernavn eksisterer allerede.");
            }

            // Tilføj brugeren til databasen
            await _users.InsertOneAsync(user);
            return Ok("Bruger oprettet.");
        }

        // Endpoint til at logge ind
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var trimmedUsername = request.Username?.Trim();
            var trimmedPassword = request.Password?.Trim();

            // Find brugeren i databasen baseret på brugernavn og password
            var existingUser = await _users.Find(u =>
                u.Username.ToLower() == trimmedUsername.ToLower() &&
                u.Password == trimmedPassword
            ).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return Unauthorized("Forkert brugernavn eller kodeord.");
            }

            // Returner UserId som en string i stedet for ObjectId
            return Ok(new { userId = existingUser.Id.ToString(), message = "Login succesfuldt" });
        }
    }
}