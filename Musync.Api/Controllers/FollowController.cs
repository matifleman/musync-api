using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.DTOs;
using Musync.Application.Features.Follow.Commands.FollowUser;
using Musync.Application.Features.Follow.Commands.UnfollowUser;

namespace Musync.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/follow")]
    public class FollowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FollowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Sigue a un usuario por su ID.
        /// </summary>
        /// <param name="userId">ID del usuario a seguir.</param>
        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(FollowResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FollowResultDTO>> FollowUser([FromRoute] int userId)
        {
            var result = await _mediator.Send(new FollowUserCommand(userId));
            return Ok(result);
        }

        /// <summary>
        /// Deja de seguir a un usuario por su ID.
        /// </summary>
        /// <param name="userId">ID del usuario a dejar de seguir.</param>
        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(FollowResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FollowResultDTO>> UnfollowUser([FromRoute] int userId)
        {
            var result = await _mediator.Send(new UnfollowUserCommand(userId));
            return Ok(result);
        }
    }
}
