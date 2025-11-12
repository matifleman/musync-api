using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.User.Queries.SearchUsers
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, List<UserSearchDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public SearchUsersQueryHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<List<UserSearchDTO>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            // IMPORTANTE: Intentar obtener el usuario actual, pero NO fallar si no está autenticado
            ApplicationUser? currentUser = null;
            int? currentUserId = null;

            try
            {
                currentUser = await _currentUserService.GetCurrentUserAsync();
                currentUserId = currentUser?.Id;
            }
            catch
            {
                // Si falla (usuario no autenticado), continuar sin usuario
                // La búsqueda funcionará igual, solo que isFollowed será false para todos
            }

            // Si el término de búsqueda está vacío, retornar lista vacía
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                return new List<UserSearchDTO>();
            }

            // Normalizar el término de búsqueda
            var searchTermLower = request.SearchTerm.Trim().ToLower();

            // Query base: buscar usuarios que coincidan con el username
            var query = _userManager.Users
                .Include(u => u.Followers)
                .Where(u => u.UserName.ToLower().Contains(searchTermLower))
                .AsQueryable();

            // Excluir al usuario actual de los resultados si está autenticado
            if (currentUserId.HasValue)
            {
                query = query.Where(u => u.Id != currentUserId.Value);
            }

            // Ordenar por relevancia:
            // 1. Usuarios que empiezan con el término de búsqueda primero
            // 2. Luego los que lo contienen en cualquier parte
            // 3. Por cantidad de seguidores (más populares primero)
            var users = await query
                .OrderByDescending(u => u.UserName.ToLower().StartsWith(searchTermLower))
                .ThenByDescending(u => u.Followers!.Count)
                .ThenBy(u => u.UserName)
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
                    IsFollowed = currentUserId.HasValue &&
                                 u.Followers!.Any(f => f.Id == currentUserId.Value)
                })
                .ToListAsync(cancellationToken);

            return users;
        }
    }
}
