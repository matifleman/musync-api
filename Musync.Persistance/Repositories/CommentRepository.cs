using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly MusyncDbContext _dbContext;

        public CommentRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Comment>> GetCommentsForPostAsync(int postId, int pageNumber, int pageSize)
        {
            // Newest first, so page 1 is what the modal opens on. Ordered by Id for the same
            // reason as GetFeedAsync: SQLite can't translate ORDER BY on DateTimeOffset, and
            // Id follows insertion order.
            return _dbContext.Comments
                .Where(comment => comment.PostId == postId)
                .Include(comment => comment.Author)
                .OrderByDescending(comment => comment.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // The inherited GetByIdAsync doesn't Include the author, but CommentDTO.Author is
        // required - same reason PostRepository has GetPostWithAuthorAsync.
        public Task<Comment?> GetCommentWithAuthorAsync(int commentId)
        {
            return _dbContext.Comments
                .AsNoTracking()
                .Include(comment => comment.Author)
                .FirstOrDefaultAsync(comment => comment.Id == commentId);
        }

        public Task<int> GetCommentsCountAsync(int postId)
        {
            return _dbContext.Comments.CountAsync(comment => comment.PostId == postId);
        }

        // One grouped query for a whole page of posts, so the feed doesn't issue a count per
        // row. Posts with no comments are simply absent from the dictionary.
        public async Task<Dictionary<int, int>> GetCommentsCountsAsync(IEnumerable<int> postIds)
        {
            return await _dbContext.Comments
                .Where(comment => postIds.Contains(comment.PostId))
                .GroupBy(comment => comment.PostId)
                .Select(group => new { PostId = group.Key, Count = group.Count() })
                .ToDictionaryAsync(entry => entry.PostId, entry => entry.Count);
        }
    }
}
