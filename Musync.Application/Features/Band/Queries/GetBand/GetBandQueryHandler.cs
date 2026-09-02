using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Queries.GetBand
{
    public sealed class GetBandQueryHandler : IRequestHandler<GetBandQuery, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBandFollowRepository _bandFollowRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetBandQueryHandler(
            IBandRepository bandRepository,
            IBandFollowRepository bandFollowRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _bandFollowRepository = bandFollowRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(GetBandQuery request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            int followersCount = await _bandFollowRepository.GetFollowersCountAsync(request.BandId);
            bool isFollowedByCurrentUser = await _bandFollowRepository.IsFollowingAsync(request.BandId, _currentUserService.CurrentUserId);

            return BandMapper.ToBandDto(band, followersCount, isFollowedByCurrentUser);
        }
    }
}
