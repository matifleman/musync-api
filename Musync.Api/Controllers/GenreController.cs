using MediatR;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.Features.Genre.Queries;

namespace Musync.Api.Controllers
{
    [Route("api/genres")]
    public sealed class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GenreDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GenreDTO>>> GetGenres()
        {
            List<GenreDTO> genres = await _mediator.Send(new GetGenresQuery());
            return Ok(genres);
        }
    }
}
