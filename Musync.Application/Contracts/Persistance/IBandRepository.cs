using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IBandRepository : IGenericRepository<Band>
    {
        Task<List<Band>> SearchAsync(string? searchTerm, int? instrumentId, int? genreId, int pageNumber, int pageSize);
        Task<Band?> GetBandWithDetailsAsync(int bandId);
        Task<List<Band>> GetBandsByUserIdAsync(int userId);
    }
}
