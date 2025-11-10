using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(MusyncDbContext db) : base(db)
        {
        }
    }
}
