using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Discover.Queries.GetSuggestedUsers
{
    public sealed record GetSuggestedUsersQuery(int PageNumber = 1, int PageSize = 20) : IRequest<List<UserSearchDTO>>;
}
