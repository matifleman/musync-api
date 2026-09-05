using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IReleaseRepository : IGenericRepository<Release>
    {
        Task<List<Release>> GetByBandIdAsync(int bandId, int pageNumber, int pageSize);
        Task<Release?> GetByIdWithSongsAsync(int releaseId);
    }
}
