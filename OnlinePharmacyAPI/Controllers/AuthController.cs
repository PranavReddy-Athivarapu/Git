using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using OnlinePharmacyAppAPI.Models;

namespace OnlinePharmacyAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
        {
            var user = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(response);
        }
    }
}
