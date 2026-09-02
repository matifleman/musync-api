using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.UnfollowBand
{
    public sealed record UnfollowBandCommand(int BandId) : IRequest<BandFollowResultDTO>;
}
