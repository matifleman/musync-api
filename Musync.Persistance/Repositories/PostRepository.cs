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

        public override async Task<IReadOnlyList<Post>> GetAllAsync()
        {
            List<Post> posts = await _dbContext.Posts
                .Include(post => post.Author)
                .ToListAsync();



            return posts.OrderByDescending(post => post.CreatedAt).ToList();
        }
    }
}
