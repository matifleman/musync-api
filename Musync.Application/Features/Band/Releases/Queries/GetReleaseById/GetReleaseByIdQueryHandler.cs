using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Releases.Queries.GetReleaseById
{
    public sealed class GetReleaseByIdQueryHandler : IRequestHandler<GetReleaseByIdQuery, ReleaseDetailDTO>
    {
        private readonly IReleaseRepository _releaseRepository;

        public GetReleaseByIdQueryHandler(IReleaseRepository releaseRepository)
        {
            _releaseRepository = releaseRepository;
        }

        public async Task<ReleaseDetailDTO> Handle(GetReleaseByIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Release release = await _releaseRepository.GetByIdWithSongsAsync(request.ReleaseId)
                ?? throw new NotFoundException($"Release with id '{request.ReleaseId}' not found");

            return ReleaseMapper.ToDetailDto(release);
        }
    }
}
