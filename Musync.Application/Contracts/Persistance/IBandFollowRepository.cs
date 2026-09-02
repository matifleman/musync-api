using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IBandFollowRepository : IGenericRepository<BandFollower>
    {
        Task<bool> IsFollowingAsync(int bandId, int userId);
        Task<int> GetFollowersCountAsync(int bandId);
        Task<BandFollower?> GetFollowAsync(int bandId, int userId);
    }
}
