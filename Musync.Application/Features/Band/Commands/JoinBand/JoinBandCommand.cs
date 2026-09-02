using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.JoinBand
{
    public sealed record JoinBandCommand(int BandId, int InstrumentId) : IRequest<BandDTO>;
}
