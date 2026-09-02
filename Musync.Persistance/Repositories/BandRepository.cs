using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class BandRepository : GenericRepository<Band>, IBandRepository
    {
        private readonly MusyncDbContext _dbContext;

        public BandRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Band>> SearchByNameAsync(string searchTerm, int pageNumber, int pageSize)
        {
            string term = searchTerm.ToLower();

            return await _dbContext.Bands
                .Include(b => b.Members)
                .Where(b => b.Name.ToLower().Contains(term))
                .OrderByDescending(b => b.Name.ToLower().StartsWith(term))
                .ThenBy(b => b.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Band?> GetBandWithDetailsAsync(int bandId)
        {
            return await _dbContext.Bands
                .Include(b => b.RequiredInstruments)
                .Include(b => b.Genres)
                .Include(b => b.Members).ThenInclude(m => m.User)
                .Include(b => b.Members).ThenInclude(m => m.Instrument)
                .FirstOrDefaultAsync(b => b.Id == bandId);
        }

        public async Task<List<Band>> GetBandsByUserIdAsync(int userId)
        {
            return await _dbContext.Bands
                .Include(b => b.Members).ThenInclude(m => m.Instrument)
                .Where(b => b.CreatedById == userId || b.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }
    }
}
