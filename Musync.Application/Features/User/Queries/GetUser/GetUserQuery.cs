using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Queries.GetUser
{
    public sealed record GetUserQuery(int userId) : IRequest<UserDTO>;
}
