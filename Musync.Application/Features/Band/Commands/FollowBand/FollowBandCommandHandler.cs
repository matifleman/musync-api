using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.FollowBand
{
    public sealed class FollowBandCommandHandler : IRequestHandler<FollowBandCommand, BandFollowResultDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBandFollowRepository _bandFollowRepository;
        private readonly ICurrentUserService _currentUserService;

        public FollowBandCommandHandler(
            IBandRepository bandRepository,
            IBandFollowRepository bandFollowRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _bandFollowRepository = bandFollowRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandFollowResultDTO> Handle(FollowBandCommand request, CancellationToken cancellationToken)
        {
            _ = await _bandRepository.GetByIdAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            int currentUserId = _currentUserService.CurrentUserId;

            if (await _bandFollowRepository.IsFollowingAsync(request.BandId, currentUserId))
                throw new BadRequestException("You already follow this band");

            await _bandFollowRepository.CreateAsync(new Domain.BandFollower
            {
                BandId = request.BandId,
                UserId = currentUserId
            });

            return new BandFollowResultDTO
            {
                BandId = request.BandId,
                IsFollowing = true,
                FollowersCount = await _bandFollowRepository.GetFollowersCountAsync(request.BandId)
            };
        }
    }
}
