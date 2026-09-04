using MediatR;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Band.Queries.GetFollowedBandsCount
{
    public sealed class GetFollowedBandsCountQueryHandler : IRequestHandler<GetFollowedBandsCountQuery, FollowedBandsCountDTO>
    {
        private readonly IBandFollowRepository _bandFollowRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetFollowedBandsCountQueryHandler(IBandFollowRepository bandFollowRepository, UserManager<ApplicationUser> userManager)
        {
            _bandFollowRepository = bandFollowRepository;
            _userManager = userManager;
        }

        public async Task<FollowedBandsCountDTO> Handle(GetFollowedBandsCountQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                throw new NotFoundException($"User with id '{request.UserId}' not found");

            int count = await _bandFollowRepository.GetFollowedBandsCountAsync(request.UserId);

            return new FollowedBandsCountDTO { UserId = request.UserId, FollowedBandsCount = count };
        }
    }
}
