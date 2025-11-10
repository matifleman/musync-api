using MediatR;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.Features.Instrument.Queries;

namespace Musync.Api.Controllers
{
    [Route("api/instrument")]
    public sealed class InstrumentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstrumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InstrumentDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<InstrumentDTO>>> GetInstruments()
        {
            IReadOnlyList<InstrumentDTO> instruments = await _mediator.Send(new GetInstrumentsCommand());
            return Ok(instruments);
        }
    }
}
