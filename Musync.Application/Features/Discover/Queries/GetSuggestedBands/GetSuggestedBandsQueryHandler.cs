using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.Discover.Queries.GetSuggestedBands
{
    public sealed class GetSuggestedBandsQueryHandler : IRequestHandler<GetSuggestedBandsQuery, List<BandSearchDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBandRepository _bandRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetSuggestedBandsQueryHandler(
            UserManager<ApplicationUser> userManager,
            IBandRepository bandRepository,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _bandRepository = bandRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<BandSearchDTO>> Handle(GetSuggestedBandsQuery request, CancellationToken cancellationToken)
        {
            int currentUserId = _currentUserService.CurrentUserId;

            // The caller's side comes from UserManager like the other user reads; the band side
            // goes through the repository like every other band query.
            List<int> myGenreIds = await _userManager.Users
                .Where(u => u.Id == currentUserId)
                .SelectMany(u => u.FavoriteGenres!.Select(g => g.Id))
                .ToListAsync(cancellationToken);

            List<Domain.Band> bands = await _bandRepository.GetSuggestedAsync(
                currentUserId, myGenreIds, request.PageNumber, request.PageSize);

            return bands
                .Select(b => new BandSearchDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    MemberCount = b.Members.Count,
                    ProfilePicture = b.ProfilePicture
                })
                .ToList();
        }
    }
}
