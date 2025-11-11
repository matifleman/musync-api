using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Follow.Commands.FollowUser
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, FollowResultDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public FollowUserCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<FollowResultDTO> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            // Obtener usuario autenticado
            var currentUser = await _currentUserService.GetCurrentUserAsync()
                ?? throw new UnauthorizedAccessException("Usuario no autenticado");

            // No puede seguirse a sí mismo
            if (currentUser.Id == request.UserIdToFollow)
                throw new BadRequestException("No puedes seguirte a ti mismo");

            // Cargar usuario actual con su lista de seguidos
            var fullCurrentUser = await _userManager.Users
                .Include(u => u.Followed)
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken)
                ?? throw new NotFoundException("Usuario actual no encontrado");

            // Cargar el usuario a seguir con su lista de seguidores
            var userToFollow = await _userManager.Users
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == request.UserIdToFollow, cancellationToken)
                ?? throw new NotFoundException("Usuario a seguir no encontrado");

            // Verificar si ya lo sigue
            if (fullCurrentUser.IsFollowing(request.UserIdToFollow))
                throw new BadRequestException("Ya sigues a este usuario");

            // Agregar la relación
            fullCurrentUser.Followed!.Add(userToFollow);
            userToFollow.Followers!.Add(fullCurrentUser);

            // Guardar cambios
            await _userManager.UpdateAsync(fullCurrentUser);
            await _userManager.UpdateAsync(userToFollow);

            // Retornar DTO
            return new FollowResultDTO
            {
                UserId = userToFollow.Id,
                IsFollowing = true,
                FollowersCount = userToFollow.Followers!.Count,
                FollowingCount = fullCurrentUser.Followed!.Count
            };
        }
    }
}
