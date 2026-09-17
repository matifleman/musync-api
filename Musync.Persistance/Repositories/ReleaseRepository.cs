using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class ReleaseRepository : GenericRepository<Release>, IReleaseRepository
    {
        private readonly MusyncDbContext _dbContext;

        public ReleaseRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Release>> GetByBandIdAsync(int bandId, int pageNumber, int pageSize)
        {
            return _dbContext.Releases
                .Where(r => r.BandId == bandId)
                .OrderByDescending(r => r.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<Release?> GetByIdWithSongsAsync(int releaseId)
        {
            return _dbContext.Releases
                .Include(r => r.Songs)
                .FirstOrDefaultAsync(r => r.Id == releaseId);
        }
    }
}
