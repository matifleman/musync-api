using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Features.User.Queries.GetUser;
using Musync.Application.Features.User.Queries.GetUsers;
using Musync.Application.Features.User.Queries.SearchUsers;
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

        /// <summary>
        /// Buscar usuarios por username
        /// </summary>
        /// <param name="q">Término de búsqueda (username)</param>
        /// <param name="pageNumber">Número de página (default: 1)</param>
        /// <param name="pageSize">Tamaño de página (default: 20, max: 50)</param>
        /// <returns>Lista de usuarios que coinciden con la búsqueda</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<UserSearchDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserSearchDTO>>> SearchUsers(
            [FromQuery] string q,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            // Validar y limitar el tamaño de página
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            var result = await _mediator.Send(new SearchUsersQuery(q, pageNumber, pageSize));
            return Ok(result);
        }
    }
}
