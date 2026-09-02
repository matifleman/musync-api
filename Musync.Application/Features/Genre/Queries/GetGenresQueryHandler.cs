using AutoMapper;
using MediatR;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.Genre.Queries
{
    public sealed class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, List<GenreDTO>>
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GetGenresQueryHandler(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<List<GenreDTO>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Domain.Genre> genres = await _genreRepository.GetAllAsync();
            return _mapper.Map<List<GenreDTO>>(genres);
        }
    }
}
