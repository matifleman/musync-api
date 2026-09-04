using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.GetFollowedBands
{
    public sealed record GetFollowedBandsQuery(int UserId, int PageNumber = 1, int PageSize = 20) : IRequest<List<FollowedBandDTO>>;
}
