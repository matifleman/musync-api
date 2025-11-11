using MediatR;

namespace Musync.Application.Features.Instrument.Queries
{
    public sealed class GetInstrumentsQuery : IRequest<List<InstrumentDTO>>
    {
    }
}
