using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Queries.GetUserFollowing
{
    public sealed record GetUserFollowingQuery(int UserId, int PageNumber = 1, int PageSize = 20) : IRequest<List<UserSearchDTO>>;
}
