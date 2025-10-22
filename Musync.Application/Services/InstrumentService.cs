using MapsterMapper;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Services
{
    public sealed class InstrumentService : BaseService<InstrumentDTO, Instrument>, IInstrumentService
    {
        public InstrumentService(IMapper mapper, IGenericRepository<Instrument> genericRepository) : base(mapper, genericRepository)
        {
        }
    }
}
