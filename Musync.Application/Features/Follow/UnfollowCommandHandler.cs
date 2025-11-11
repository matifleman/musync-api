using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Follow.Commands.UnfollowUser
{
    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, FollowResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public UnfollowUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<FollowResultDTO> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            // Obtener el usuario actual
            var currentUser = await _currentUserService.GetCurrentUserAsync()
                ?? throw new UnauthorizedAccessException("Usuario no autenticado");

            if (currentUser.Id == request.UserIdToUnfollow)
                throw new BadRequestException("No puedes dejar de seguirte a ti mismo");

            // Cargar el usuario actual con su lista de seguidos
            var fullCurrentUser = await _userManager.Users
                .Include(u => u.Followed)
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken)
                ?? throw new NotFoundException("Usuario actual no encontrado");

            // Cargar el usuario a dejar de seguir con su lista de seguidores
            var userToUnfollow = await _userManager.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == request.UserIdToUnfollow, cancellationToken)
                ?? throw new NotFoundException("Usuario no encontrado");

            // Verificar si realmente lo sigue
            if (!fullCurrentUser.IsFollowing(request.UserIdToUnfollow))
                throw new BadRequestException("No sigues a este usuario");

            // Remover la relación
            fullCurrentUser.Followed!.Remove(userToUnfollow);
            userToUnfollow.Followers!.Remove(fullCurrentUser);

            // Guardar cambios
            await _userManager.UpdateAsync(fullCurrentUser);
            await _userManager.UpdateAsync(userToUnfollow);

            return new FollowResultDTO
            {
                UserId = userToUnfollow.Id,
                IsFollowing = false,
                FollowersCount = userToUnfollow.Followers!.Count,
                FollowingCount = fullCurrentUser.Followed!.Count
            };
        }
    }
}
