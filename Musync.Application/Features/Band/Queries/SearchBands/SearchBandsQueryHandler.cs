using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.SearchBands
{
    public sealed class SearchBandsQueryHandler : IRequestHandler<SearchBandsQuery, List<BandSearchDTO>>
    {
        private readonly IBandRepository _bandRepository;

        public SearchBandsQueryHandler(IBandRepository bandRepository)
        {
            _bandRepository = bandRepository;
        }

        public async Task<List<BandSearchDTO>> Handle(SearchBandsQuery request, CancellationToken cancellationToken)
        {
            bool hasNoFilters = string.IsNullOrWhiteSpace(request.SearchTerm)
                && request.InstrumentId is null
                && request.GenreId is null;

            if (hasNoFilters)
                return new List<BandSearchDTO>();

            List<Domain.Band> bands = await _bandRepository.SearchAsync(
                request.SearchTerm, request.InstrumentId, request.GenreId, request.PageNumber, request.PageSize);

            return bands
                .Select(b => new BandSearchDTO { Id = b.Id, Name = b.Name, MemberCount = b.Members.Count })
                .ToList();
        }
    }
}
