using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsForPostAsync(int postId, int pageNumber, int pageSize);
        Task<Comment?> GetCommentWithAuthorAsync(int commentId);
        Task<int> GetCommentsCountAsync(int postId);
        Task<Dictionary<int, int>> GetCommentsCountsAsync(IEnumerable<int> postIds);
    }
}
