using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.Features.Profile.Queries;
using System.Security.Claims;

namespace Musync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            // Obtener el ID del usuario actual si está autenticado
            int? currentUserId = null;
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int parsedUserId))
                {
                    currentUserId = parsedUserId;
                }
            }

            var query = new GetUserProfileQuery
            {
                UserId = userId,
                CurrentUserId = currentUserId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var query = new GetUserProfileQuery
            {
                UserId = userId,
                CurrentUserId = userId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
