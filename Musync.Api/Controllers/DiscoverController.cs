using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.DTOs;
using Musync.Application.Features.Discover.Queries.GetSuggestedBands;
using Musync.Application.Features.Discover.Queries.GetSuggestedUsers;

namespace Musync.Api.Controllers
{
    [ApiController]
    [Route("api/discover")]
    public class DiscoverController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DiscoverController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("users")]
        [ProducesResponseType(typeof(List<UserSearchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<UserSearchDTO>>> GetSuggestedUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<UserSearchDTO> users = await _mediator.Send(new GetSuggestedUsersQuery(pageNumber, pageSize));
            return Ok(users);
        }

        [Authorize]
        [HttpGet("bands")]
        [ProducesResponseType(typeof(List<BandSearchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<BandSearchDTO>>> GetSuggestedBands(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<BandSearchDTO> bands = await _mediator.Send(new GetSuggestedBandsQuery(pageNumber, pageSize));
            return Ok(bands);
        }
    }
}
