using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.LeaveBand
{
    public sealed record LeaveBandCommand(int BandId) : IRequest<BandDTO>;
}
