using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Features.User.Queries.GetUser;
using Musync.Application.Features.User.Queries.GetUsers;
using Musync.Domain;

namespace Musync.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public UserController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUser([FromRoute] int userId)
        {
            UserDTO user = await _mediator.Send(new GetUserQuery(userId));
            return Ok(user);
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> GetMyUser()
        {
            ApplicationUser currentUser = (await _currentUserService.GetCurrentUserAsync())!;
            UserDTO user = await _mediator.Send(new GetUserQuery(currentUser.Id));
            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserDTO> users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }
    }
}
