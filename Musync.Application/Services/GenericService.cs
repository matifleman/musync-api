using MapsterMapper;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Domain.Common;

namespace Musync.Application.Services
{
    public class GenericService<Dto, Entity> : IGenericService<Dto, Entity> where Dto : class where Entity : BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Entity> _genericRepository;

        public GenericService(IMapper mapper, IGenericRepository<Entity> genericRepository)
        {
            _mapper = mapper;
            _genericRepository = genericRepository;
        }
        public Task<Dto> CreateAsync(Dto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Dto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Dto>> GetAllAsync()
        {
            var listOfEntities = await _genericRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<Dto>>(listOfEntities);
        }

        public Task<Dto?> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Dto> UpdateAsync(Dto entity)
        {
            throw new NotImplementedException();
        }
    }
}
