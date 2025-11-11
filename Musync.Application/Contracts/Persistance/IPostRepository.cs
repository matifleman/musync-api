using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetPostsByAuthorIdAsync(int authorId);
    }
}
