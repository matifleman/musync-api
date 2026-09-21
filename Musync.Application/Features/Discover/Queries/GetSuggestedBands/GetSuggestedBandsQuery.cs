using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Discover.Queries.GetSuggestedBands
{
    public sealed record GetSuggestedBandsQuery(int PageNumber = 1, int PageSize = 20) : IRequest<List<BandSearchDTO>>;
}
