using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.CreateBand
{
    public sealed record CreateBandCommand(string Name, List<int> InstrumentIds) : IRequest<BandDTO>;
}
