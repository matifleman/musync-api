using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.Discover.Queries.GetSuggestedUsers
{
    public sealed class GetSuggestedUsersQueryHandler : IRequestHandler<GetSuggestedUsersQuery, List<UserSearchDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetSuggestedUsersQueryHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        // Queries UserManager.Users directly, like every other user handler - there is no
        // user repository yet (refactor-plan Group 5).
        public async Task<List<UserSearchDTO>> Handle(GetSuggestedUsersQuery request, CancellationToken cancellationToken)
        {
            int currentUserId = _currentUserService.CurrentUserId;

            List<int> myGenreIds = await _userManager.Users
                .Where(u => u.Id == currentUserId)
                .SelectMany(u => u.FavoriteGenres!.Select(g => g.Id))
                .ToListAsync(cancellationToken);

            List<int> myInstrumentIds = await _userManager.Users
                .Where(u => u.Id == currentUserId)
                .SelectMany(u => u.FavoriteInstruments!.Select(i => i.Id))
                .ToListAsync(cancellationToken);

            // One ordering covers both cases: people who share tags with you come first, and
            // everyone else follows by popularity. A caller with no tags scores 0 against
            // everybody, so the "most-followed" fallback is this same query rather than a
            // second code path - and the list never runs dry for someone with niche tastes.
            return await _userManager.Users
                .AsNoTracking()
                .Where(u => u.Id != currentUserId && !u.Followers!.Any(f => f.Id == currentUserId))
                .OrderByDescending(u =>
                    u.FavoriteGenres!.Count(g => myGenreIds.Contains(g.Id)) +
                    u.FavoriteInstruments!.Count(i => myInstrumentIds.Contains(i.Id)))
                .ThenByDescending(u => u.Followers!.Count)
                .ThenBy(u => u.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(u => new UserSearchDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    ProfilePicture = u.ProfilePicture,
                    FollowersCount = u.Followers!.Count,
                    // Always false: anyone the caller follows was filtered out above.
                    IsFollowed = false
                })
                .ToListAsync(cancellationToken);
        }
    }
}
