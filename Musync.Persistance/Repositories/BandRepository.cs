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

        public async Task<List<Band>> SearchAsync(string? searchTerm, int? instrumentId, int? genreId, int pageNumber, int pageSize)
        {
            string? term = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim().ToLower();

            IQueryable<Band> query = _dbContext.Bands.Include(b => b.Members);

            if (term is not null)
                query = query.Where(b => b.Name.ToLower().Contains(term));

            if (instrumentId.HasValue)
            {
                int id = instrumentId.Value;
                query = query.Where(b =>
                    b.RequiredInstruments.Any(i => i.Id == id) &&
                    !b.Members.Any(m => m.InstrumentId == id));
            }

            if (genreId.HasValue)
            {
                int id = genreId.Value;
                query = query.Where(b => b.Genres.Any(g => g.Id == id));
            }

            query = term is not null
                ? query.OrderByDescending(b => b.Name.ToLower().StartsWith(term)).ThenBy(b => b.Name)
                : query.OrderBy(b => b.Name);

            return await query
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
