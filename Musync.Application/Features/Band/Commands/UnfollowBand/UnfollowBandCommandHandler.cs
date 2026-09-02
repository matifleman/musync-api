using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.UnfollowBand
{
    public sealed class UnfollowBandCommandHandler : IRequestHandler<UnfollowBandCommand, BandFollowResultDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBandFollowRepository _bandFollowRepository;
        private readonly ICurrentUserService _currentUserService;

        public UnfollowBandCommandHandler(
            IBandRepository bandRepository,
            IBandFollowRepository bandFollowRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _bandFollowRepository = bandFollowRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandFollowResultDTO> Handle(UnfollowBandCommand request, CancellationToken cancellationToken)
        {
            _ = await _bandRepository.GetByIdAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            int currentUserId = _currentUserService.CurrentUserId;

            Domain.BandFollower follow = await _bandFollowRepository.GetFollowAsync(request.BandId, currentUserId)
                ?? throw new BadRequestException("You do not follow this band");

            await _bandFollowRepository.DeleteAsync(follow);

            return new BandFollowResultDTO
            {
                BandId = request.BandId,
                IsFollowing = false,
                FollowersCount = await _bandFollowRepository.GetFollowersCountAsync(request.BandId)
            };
        }
    }
}
