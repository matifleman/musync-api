using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musync.Application.Features.Profile.Queries
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDTO> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(u => u.Followers)
                .Include(u => u.Followed)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }

            bool esSeguido = false;
            if (request.CurrentUserId.HasValue && request.CurrentUserId != request.UserId)
            {
                esSeguido = user.Followers?.Any(s => s.Id == request.CurrentUserId.Value) ?? false;
            }

            return new UserProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = user.ProfilePicture,
                FollowersCount = user.Followers?.Count ?? 0,
                FollowedCount = user.Followed?.Count ?? 0,
                IsFollowed = user.IsFollowed
            };
        }
    }
}
