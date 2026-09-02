using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.UpdateBandInstruments
{
    public sealed record UpdateBandInstrumentsCommand(int BandId, List<int> InstrumentIds) : IRequest<BandDTO>;
}
