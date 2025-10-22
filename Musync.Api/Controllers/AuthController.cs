using Microsoft.AspNetCore.Mvc;
using Musync.Api.Models;
using Musync.Application.Contracts.Identity;
using Musync.Application.Models.Identity;

namespace Musync.Api.Controllers
{
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegistrationRequest request)
        {
            AuthResponse authResponse = await _authService.Register(request);
            return Ok(authResponse);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            AuthResponse authResponse = await _authService.Login(request);
            return Ok(authResponse);
        }
    }
}
