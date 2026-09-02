using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.GetUserBands
{
    public sealed record GetUserBandsQuery(int UserId) : IRequest<List<UserBandDTO>>;
}
