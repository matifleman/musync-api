using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class BandMemberRepository : GenericRepository<BandMember>, IBandMemberRepository
    {
        private readonly MusyncDbContext _dbContext;

        public BandMemberRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BandMember?> GetMembershipOfUserAsync(int userId, int bandId)
        {
            return await _dbContext.BandMembers
                .FirstOrDefaultAsync(bm => bm.UserId == userId && bm.BandId == bandId);
        }

        public Task<bool> IsInstrumentTakenAsync(int bandId, int instrumentId)
        {
            return _dbContext.BandMembers
                .AnyAsync(bm => bm.BandId == bandId && bm.InstrumentId == instrumentId);
        }
    }
}
