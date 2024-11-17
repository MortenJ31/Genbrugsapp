using Core;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {
            // Tjek om brugernavn allerede eksisterer
            var existingUser = await _userService.GetUserByUsernameAsync(newUser.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Brugernavn eksisterer allerede." });
            }

            await _userService.AddUserAsync(newUser);
            return Ok(new { message = "Bruger oprettet." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            
            var user = await _userService.GetUserByUsernameAsync(request.Username);

            if (user == null || user.Password != request.Password)
            {
                return Unauthorized(new { message = "Forkert brugernavn eller kodeord." });
            }

            return Ok(new { userId = user.UserId, message = "Login succesfuldt" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User updatedUser)
        {
            await _userService.UpdateUserAsync(id, updatedUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
