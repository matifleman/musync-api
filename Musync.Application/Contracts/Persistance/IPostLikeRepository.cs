using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {
        Task<PostLike?> GetLikeOfUser(int userId, int postId);
        Task<bool> HasUserLikedPost(int userId, int postId);
        Task<HashSet<int>> GetLikedPostIdsAsync(int userId, IEnumerable<int> postIds);
    }
}
