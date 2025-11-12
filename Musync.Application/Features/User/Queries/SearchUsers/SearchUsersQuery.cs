using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Queries.SearchUsers
{
    public record SearchUsersQuery(string SearchTerm, int PageNumber = 1, int PageSize = 20)
        : IRequest<List<UserSearchDTO>>;
}