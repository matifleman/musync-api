using Microsoft.AspNetCore.Mvc;
using Musync.Application.Contracts.Identity;
using Musync.Application.Models.Identity;

namespace Musync.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            AuthResponse authResponse = await _authService.Register(request);
            return Ok(authResponse);
        }
    }
}
