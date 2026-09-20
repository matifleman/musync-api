using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetPostsByAuthorIdAsync(int authorId);
        Task<List<Post>> GetFeedAsync(int userId, int pageNumber, int pageSize);
        Task<Post?> GetPostWithAuthorAsync(int postId);
    }
}
