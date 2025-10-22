using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MusyncDbContext dbContext) : base(dbContext)
        {
        }
    }
}
