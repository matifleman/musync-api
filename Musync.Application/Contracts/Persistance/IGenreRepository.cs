using Musync.Domain;

namespace Musync.Application.Contracts.Persistance
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<List<Genre>> GetByIdsAsync(List<int> ids);
    }
}
