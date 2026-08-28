using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Queries.GetUsers
{
    public sealed record GetUsersQuery(int PageNumber = 1, int PageSize = 20) : IRequest<List<UserDTO>>;
}
