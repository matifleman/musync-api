using Musync.Application.Features.Instrument.Queries;
using Musync.Domain;

namespace Musync.Application.Contracts.Services
{
    public interface IInstrumentService : IBaseService<InstrumentDTO, Instrument>
    {
    }
}
