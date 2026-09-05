using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Commands.UpdateGenres
{
    public sealed record UpdateGenresCommand(List<int> GenreIds) : IRequest<CurrentUserDTO>;
}
