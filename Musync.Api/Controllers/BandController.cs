using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Api.Models;
using Musync.Application.DTOs;
using Musync.Application.Features.Band.Commands.CreateBand;
using Musync.Application.Features.Band.Commands.JoinBand;
using Musync.Application.Features.Band.Commands.LeaveBand;
using Musync.Application.Features.Band.Commands.UpdateBandName;
using Musync.Application.Features.Band.Queries.GetBand;
using Musync.Application.Features.Band.Queries.SearchBands;

namespace Musync.Api.Controllers
{
    [ApiController]
    [Route("api/bands")]
    public class BandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BandDTO>> CreateBand([FromBody] CreateBandCommand command)
        {
            BandDTO created = await _mediator.Send(command);
            return Created($"/api/bands/{created.Id}", created);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(List<BandSearchDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BandSearchDTO>>> SearchBands(
            [FromQuery] string q,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<BandSearchDTO> result = await _mediator.Send(new SearchBandsQuery(q, pageNumber, pageSize));
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{bandId}")]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> GetBand([FromRoute] int bandId)
        {
            BandDTO band = await _mediator.Send(new GetBandQuery(bandId));
            return Ok(band);
        }

        [Authorize]
        [HttpPost("{bandId}/join")]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> JoinBand([FromRoute] int bandId, [FromBody] JoinBandRequest request)
        {
            BandDTO band = await _mediator.Send(new JoinBandCommand(bandId, request.InstrumentId));
            return Ok(band);
        }

        [Authorize]
        [HttpDelete("{bandId}/membership")]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> LeaveBand([FromRoute] int bandId)
        {
            BandDTO band = await _mediator.Send(new LeaveBandCommand(bandId));
            return Ok(band);
        }

        [Authorize]
        [HttpPut("{bandId}/name")]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> UpdateBandName([FromRoute] int bandId, [FromBody] UpdateBandNameRequest request)
        {
            BandDTO band = await _mediator.Send(new UpdateBandNameCommand(bandId, request.Name));
            return Ok(band);
        }
    }
}
