using MediatR;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Band.Queries.GetFollowedBands
{
    public sealed class GetFollowedBandsQueryHandler : IRequestHandler<GetFollowedBandsQuery, List<FollowedBandDTO>>
    {
        private readonly IBandFollowRepository _bandFollowRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetFollowedBandsQueryHandler(IBandFollowRepository bandFollowRepository, UserManager<ApplicationUser> userManager)
        {
            _bandFollowRepository = bandFollowRepository;
            _userManager = userManager;
        }

        public async Task<List<FollowedBandDTO>> Handle(GetFollowedBandsQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                throw new NotFoundException($"User with id '{request.UserId}' not found");

            List<BandFollower> follows = await _bandFollowRepository.GetFollowedBandsAsync(request.UserId, request.PageNumber, request.PageSize);

            return follows.Select(bf => new FollowedBandDTO
            {
                Id = bf.Band!.Id,
                Name = bf.Band.Name,
                ProfilePicture = bf.Band.ProfilePicture,
                MemberCount = bf.Band.Members.Count
            }).ToList();
        }
    }
}
