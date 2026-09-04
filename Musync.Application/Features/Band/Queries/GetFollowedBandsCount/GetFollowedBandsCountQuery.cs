using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.GetFollowedBandsCount
{
    public sealed record GetFollowedBandsCountQuery(int UserId) : IRequest<FollowedBandsCountDTO>;
}
