using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IPostLikeRepository : IGenericRepository<PostLike>
    {
        Task<PostLike?> GetLikeOfUser(int userId, int postId);
        Task<bool> HasUserLikedPost(int userId, int postId);
        Task<IReadOnlyList<PostLike>> GetLikesByUserIdAsync(int userId);
    }
}
