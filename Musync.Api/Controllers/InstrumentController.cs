using Microsoft.AspNetCore.Mvc;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;

namespace Musync.Api.Controllers
{
    [Route("api/instrument")]
    public sealed class InstrumentController : ControllerBase
    {
        private readonly IInstrumentService _instrumentService;

        public InstrumentController(IInstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InstrumentDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<InstrumentDTO>>> GetInstruments()
        {
            IReadOnlyList<InstrumentDTO> instruments = await _instrumentService.GetAllAsync();
            return Ok(instruments);
        }
    }
}
