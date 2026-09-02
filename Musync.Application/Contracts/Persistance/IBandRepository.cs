using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IBandRepository : IGenericRepository<Band>
    {
        Task<List<Band>> SearchByNameAsync(string searchTerm, int pageNumber, int pageSize);
        Task<Band?> GetBandWithDetailsAsync(int bandId);
        Task<List<Band>> GetBandsByUserIdAsync(int userId);
    }
}
