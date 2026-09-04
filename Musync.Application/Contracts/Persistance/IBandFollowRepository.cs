using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IBandFollowRepository : IGenericRepository<BandFollower>
    {
        Task<bool> IsFollowingAsync(int bandId, int userId);
        Task<int> GetFollowersCountAsync(int bandId);
        Task<int> GetFollowedBandsCountAsync(int userId);
        Task<BandFollower?> GetFollowAsync(int bandId, int userId);
        Task<List<BandFollower>> GetFollowedBandsAsync(int userId, int pageNumber, int pageSize);
        Task<List<int>> GetFollowerUserIdsAsync(int bandId, int pageNumber, int pageSize);
    }
}
