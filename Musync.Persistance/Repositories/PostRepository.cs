using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly MusyncDbContext _dbContext;

        public PostRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetPostsByAuthorIdAsync(int authorId)
        {
            List<Post> userPosts = await _dbContext.Posts
                .Where(post => post.AuthorId == authorId)
                .ToListAsync();

            return userPosts;
        }

        // Read-only: the inherited GetByIdAsync doesn't Include the author, but PostDTO.Author
        // is required. AsNoTracking also guarantees a fresh instance rather than whatever the
        // identity map already holds, which matters when a handler re-reads a post it just wrote.
        public Task<Post?> GetPostWithAuthorAsync(int postId)
        {
            return _dbContext.Posts
                .AsNoTracking()
                .Include(post => post.Author)
                .FirstOrDefaultAsync(post => post.Id == postId);
        }

        public Task<List<Post>> GetFeedAsync(int userId, int pageNumber, int pageSize)
        {
            // Own posts plus posts by users the caller follows. Ordered by Id because
            // SQLite can't translate ORDER BY on DateTimeOffset; Id follows insertion order.
            return _dbContext.Posts
                .Where(post => post.AuthorId == userId || post.Author!.Followers!.Any(follower => follower.Id == userId))
                .Include(post => post.Author)
                .OrderByDescending(post => post.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
