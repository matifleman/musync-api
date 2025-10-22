using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Contracts.Services
{
    public interface IInstrumentService : IGenericService<InstrumentDTO, Instrument>
    {
    }
}
