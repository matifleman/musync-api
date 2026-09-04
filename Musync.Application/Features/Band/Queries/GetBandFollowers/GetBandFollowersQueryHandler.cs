using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Band.Queries.GetBandFollowers
{
    public sealed class GetBandFollowersQueryHandler : IRequestHandler<GetBandFollowersQuery, List<UserSearchDTO>>
    {
        private readonly IBandFollowRepository _bandFollowRepository;
        private readonly IBandRepository _bandRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetBandFollowersQueryHandler(
            IBandFollowRepository bandFollowRepository,
            IBandRepository bandRepository,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _bandFollowRepository = bandFollowRepository;
            _bandRepository = bandRepository;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<List<UserSearchDTO>> Handle(GetBandFollowersQuery request, CancellationToken cancellationToken)
        {
            Domain.Band? band = await _bandRepository.GetByIdAsync(request.BandId);
            if (band is null)
                throw new NotFoundException($"Band with id '{request.BandId}' not found");

            List<int> followerIds = await _bandFollowRepository.GetFollowerUserIdsAsync(request.BandId, request.PageNumber, request.PageSize);
            if (followerIds.Count == 0)
                return [];

            ApplicationUser currentUser = await _currentUserService.GetCurrentUserAsync();

            List<UserSearchDTO> users = await _userManager.Users
                .Where(u => followerIds.Contains(u.Id))
                .Select(u => new UserSearchDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName!,
                    ProfilePicture = u.ProfilePicture,
                    FollowersCount = u.Followers!.Count,
                    IsFollowed = u.Followers!.Any(f => f.Id == currentUser.Id)
                })
                .ToListAsync(cancellationToken);

            return followerIds.Select(id => users.First(u => u.Id == id)).ToList();
        }
    }
}
