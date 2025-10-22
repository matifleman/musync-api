using Musync.Domain.Common;

namespace Musync.Application.Contracts.Services
{
    public interface IBaseService<Dto, Entity> where Dto : class where Entity : BaseEntity
    {
        Task<IReadOnlyList<Dto>> GetAllAsync();
        Task<Dto?> GetByIdAsync(int Id);
        Task<Dto> CreateAsync(Dto entity);
        Task<Dto> UpdateAsync(Dto entity);
        Task DeleteAsync(Dto entity);
    }
}
