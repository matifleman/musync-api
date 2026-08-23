using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class InstrumentRepository : GenericRepository<Instrument>, IInstrumentRepository
    {
        private readonly MusyncDbContext _dbContext;

        public InstrumentRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Instrument>> GetByIdsAsync(List<int> ids)
        {
            return await _dbContext.Instruments
                .Where(instrument => ids.Contains(instrument.Id))
                .ToListAsync();
        }
    }
}
