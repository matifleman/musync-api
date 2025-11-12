using Musync.Domain;
using Musync.Persistance.DatabaseContext;
using Musync.Application.Contracts.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Musync.Persistance.Repositories
{
    public sealed class PostLikeRepository : GenericRepository<PostLike>, IPostLikeRepository
    {
        private readonly MusyncDbContext _dbContext;

        public PostLikeRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PostLike?> GetLikeOfUser(int userId, int postId)
        {
            return await _dbContext.PostLikes
                .Where(pl => pl.UserId == userId && pl.PostId == postId)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<PostLike>> GetLikesByUserIdAsync(int userId)
        {
            return await _dbContext.PostLikes
                .Where(pl => pl.UserId == userId)
                .ToListAsync();
        }

        public Task<bool> HasUserLikedPost(int userId, int postId)
        {
            return _dbContext.PostLikes
                .AnyAsync(pl => pl.UserId == userId && pl.PostId == postId);
        }
    }
}
