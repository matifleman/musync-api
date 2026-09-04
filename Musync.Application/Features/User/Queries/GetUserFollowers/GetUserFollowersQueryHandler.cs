using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.User.Queries.GetUserFollowers
{
    public sealed class GetUserFollowersQueryHandler : IRequestHandler<GetUserFollowersQuery, List<UserSearchDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetUserFollowersQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<List<UserSearchDTO>> Handle(GetUserFollowersQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser? targetUser = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (targetUser is null)
                throw new NotFoundException($"User with id '{request.UserId}' not found");

            ApplicationUser currentUser = await _currentUserService.GetCurrentUserAsync();

            return await _userManager.Users
                .Where(u => u.Followed!.Any(f => f.Id == request.UserId))
                .OrderByDescending(u => u.Followers!.Count)
                .ThenBy(u => u.UserName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
        }
    }
}
