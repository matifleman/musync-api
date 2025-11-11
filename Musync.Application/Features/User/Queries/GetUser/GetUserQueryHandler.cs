using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.User.Queries.GetUser
{
    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetUserQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser? targetUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.userId);

            if(targetUser is null)
                throw new NotFoundException($"User with id '{request.userId}' not found");

            ApplicationUser currentUser = (await _currentUserService.GetCurrentUserAsync())!; // won't be null because of authorization middleware

            UserDTO targetUserDto = _mapper.Map<UserDTO>(targetUser);

            targetUserDto.IsFollowed = currentUser.IsFollowing(targetUser.Id);

            return targetUserDto;
        }
    }
}
