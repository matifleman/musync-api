using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.User.Queries.GetUsers
{
    public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetUsersQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<List<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            List<ApplicationUser> users = await _userManager.Users.ToListAsync();
            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(users);

            ApplicationUser? currentUser = await _currentUserService.GetCurrentUserAsync();

            if (currentUser is not null)
                userDTOs.ForEach(userDTO => userDTO.IsFollowed = currentUser.IsFollowing(userDTO.Id));

            return userDTOs;
        }
    }
}
