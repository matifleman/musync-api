using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Releases.Queries.GetBandReleases
{
    public sealed class GetBandReleasesQueryHandler : IRequestHandler<GetBandReleasesQuery, List<ReleaseListItemDTO>>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IReleaseRepository _releaseRepository;

        public GetBandReleasesQueryHandler(IBandRepository bandRepository, IReleaseRepository releaseRepository)
        {
            _bandRepository = bandRepository;
            _releaseRepository = releaseRepository;
        }

        public async Task<List<ReleaseListItemDTO>> Handle(GetBandReleasesQuery request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetByIdAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            List<Domain.Release> releases = await _releaseRepository.GetByBandIdAsync(band.Id, request.PageNumber, request.PageSize);

            return releases.Select(ReleaseMapper.ToListItemDto).ToList();
        }
    }
}
