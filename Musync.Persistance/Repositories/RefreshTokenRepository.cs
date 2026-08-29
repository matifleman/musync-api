using Musync.Domain;
using Musync.Persistance.DatabaseContext;
using Musync.Application.Contracts.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Musync.Persistance.Repositories
{
    public sealed class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly MusyncDbContext _dbContext;

        public RefreshTokenRepository(MusyncDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            return _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        }
    }
}
