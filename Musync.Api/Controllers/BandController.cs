using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Api.Models;
using Musync.Application.Common;
using Musync.Application.DTOs;
using Musync.Application.Features.Band.Commands.CreateBand;
using Musync.Application.Features.Band.Commands.FollowBand;
using Musync.Application.Features.Band.Commands.JoinBand;
using Musync.Application.Features.Band.Commands.LeaveBand;
using Musync.Application.Features.Band.Commands.RemoveBandMember;
using Musync.Application.Features.Band.Commands.UnfollowBand;
using Musync.Application.Features.Band.Commands.UpdateBandGenres;
using Musync.Application.Features.Band.Commands.UpdateBandInstruments;
using Musync.Application.Features.Band.Commands.UpdateBandName;
using Musync.Application.Features.Band.Commands.UpdateBandPicture;
using Musync.Application.Features.Band.Queries.GetBand;
using Musync.Application.Features.Band.Queries.GetBandFollowers;
using Musync.Application.Features.Band.Queries.GetFollowedBands;
using Musync.Application.Features.Band.Queries.GetFollowedBandsCount;
using Musync.Application.Features.Band.Queries.GetUserBands;
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
            [FromQuery] string? q = null,
            [FromQuery] int? instrumentId = null,
            [FromQuery] int? genreId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<BandSearchDTO> result = await _mediator.Send(new SearchBandsQuery(q, instrumentId, genreId, pageNumber, pageSize));
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
        [HttpPost("{bandId}/follow")]
        [ProducesResponseType(typeof(BandFollowResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandFollowResultDTO>> FollowBand([FromRoute] int bandId)
        {
            BandFollowResultDTO result = await _mediator.Send(new FollowBandCommand(bandId));
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{bandId}/follow")]
        [ProducesResponseType(typeof(BandFollowResultDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandFollowResultDTO>> UnfollowBand([FromRoute] int bandId)
        {
            BandFollowResultDTO result = await _mediator.Send(new UnfollowBandCommand(bandId));
            return Ok(result);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<UserBandDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserBandDTO>>> GetUserBands([FromRoute] int userId)
        {
            List<UserBandDTO> bands = await _mediator.Send(new GetUserBandsQuery(userId));
            return Ok(bands);
        }

        [Authorize]
        [HttpGet("user/{userId}/followed-count")]
        [ProducesResponseType(typeof(FollowedBandsCountDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FollowedBandsCountDTO>> GetFollowedBandsCount([FromRoute] int userId)
        {
            FollowedBandsCountDTO result = await _mediator.Send(new GetFollowedBandsCountQuery(userId));
            return Ok(result);
        }

        [Authorize]
        [HttpGet("user/{userId}/followed")]
        [ProducesResponseType(typeof(List<FollowedBandDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<FollowedBandDTO>>> GetFollowedBands(
            [FromRoute] int userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<FollowedBandDTO> bands = await _mediator.Send(new GetFollowedBandsQuery(userId, pageNumber, pageSize));
            return Ok(bands);
        }

        [Authorize]
        [HttpGet("{bandId}/followers")]
        [ProducesResponseType(typeof(List<UserSearchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserSearchDTO>>> GetBandFollowers(
            [FromRoute] int bandId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<UserSearchDTO> followers = await _mediator.Send(new GetBandFollowersQuery(bandId, pageNumber, pageSize));
            return Ok(followers);
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
        [HttpDelete("{bandId}/members/{userId}")]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> RemoveBandMember([FromRoute] int bandId, [FromRoute] int userId)
        {
            BandDTO band = await _mediator.Send(new RemoveBandMemberCommand(bandId, userId));
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

        [Authorize]
        [HttpPut("{bandId}/picture")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(ImageUploadValidator.MaxFileSizeBytes + 1024 * 1024)]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> UpdateBandPicture([FromRoute] int bandId, [FromForm] UpdateBandPictureRequest request)
        {
            BandDTO band = await _mediator.Send(new UpdateBandPictureCommand(bandId, request.Picture));
            return Ok(band);
        }

        [Authorize]
        [HttpPut("{bandId}/genres")]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> UpdateBandGenres([FromRoute] int bandId, [FromBody] UpdateBandGenresRequest request)
        {
            BandDTO band = await _mediator.Send(new UpdateBandGenresCommand(bandId, request.GenreIds));
            return Ok(band);
        }

        [Authorize]
        [HttpPut("{bandId}/instruments")]
        [ProducesResponseType(typeof(BandDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BandDTO>> UpdateBandInstruments([FromRoute] int bandId, [FromBody] UpdateBandInstrumentsRequest request)
        {
            BandDTO band = await _mediator.Send(new UpdateBandInstrumentsCommand(bandId, request.InstrumentIds));
            return Ok(band);
        }
    }
}
