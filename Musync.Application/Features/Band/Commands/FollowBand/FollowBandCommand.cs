using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.FollowBand
{
    public sealed record FollowBandCommand(int BandId) : IRequest<BandFollowResultDTO>;
}
