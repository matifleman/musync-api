using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Domain;
using Musync.Persistance.DatabaseContext;

namespace Musync.Persistance.Repositories
{
    public sealed class BandFollowRepository : GenericRepository<BandFollower>, IBandFollowRepository
    {
        private readonly MusyncDbContext _dbContext;

        public BandFollowRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> IsFollowingAsync(int bandId, int userId)
        {
            return _dbContext.BandFollowers.AnyAsync(bf => bf.BandId == bandId && bf.UserId == userId);
        }

        public Task<int> GetFollowersCountAsync(int bandId)
        {
            return _dbContext.BandFollowers.CountAsync(bf => bf.BandId == bandId);
        }

        public Task<int> GetFollowedBandsCountAsync(int userId)
        {
            return _dbContext.BandFollowers.CountAsync(bf => bf.UserId == userId);
        }

        public Task<BandFollower?> GetFollowAsync(int bandId, int userId)
        {
            return _dbContext.BandFollowers.FirstOrDefaultAsync(bf => bf.BandId == bandId && bf.UserId == userId);
        }

        public Task<List<BandFollower>> GetFollowedBandsAsync(int userId, int pageNumber, int pageSize)
        {
            return _dbContext.BandFollowers
                .Where(bf => bf.UserId == userId)
                .Include(bf => bf.Band).ThenInclude(b => b!.Members)
                .OrderByDescending(bf => bf.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<List<int>> GetFollowerUserIdsAsync(int bandId, int pageNumber, int pageSize)
        {
            return _dbContext.BandFollowers
                .Where(bf => bf.BandId == bandId)
                .OrderByDescending(bf => bf.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(bf => bf.UserId)
                .ToListAsync();
        }
    }
}
