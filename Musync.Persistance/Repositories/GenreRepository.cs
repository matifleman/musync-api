using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private readonly MusyncDbContext _dbContext;

        public GenreRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Genre>> GetByIdsAsync(List<int> ids)
        {
            return await _dbContext.Genres
                .Where(genre => ids.Contains(genre.Id))
                .ToListAsync();
        }
    }
}
