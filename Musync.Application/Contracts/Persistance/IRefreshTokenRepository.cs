using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    }
}
