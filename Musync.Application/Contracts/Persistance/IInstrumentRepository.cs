using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IInstrumentRepository : IGenericRepository<Instrument>
    {
        Task<List<Instrument>> GetByIdsAsync(List<int> ids);
    }
}
