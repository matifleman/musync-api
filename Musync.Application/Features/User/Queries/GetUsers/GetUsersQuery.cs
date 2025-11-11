using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Queries.GetUsers
{
    public sealed record GetUsersQuery : IRequest<List<UserDTO>>;
}
