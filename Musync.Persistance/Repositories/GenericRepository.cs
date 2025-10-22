using Musync.Domain.Common;
using Musync.Persistance.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;

namespace Musync.Persistance.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MusyncDbContext _dbContext;

        public GenericRepository(MusyncDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
