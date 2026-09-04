using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.GetBandFollowers
{
    public sealed record GetBandFollowersQuery(int BandId, int PageNumber = 1, int PageSize = 20) : IRequest<List<UserSearchDTO>>;
}
