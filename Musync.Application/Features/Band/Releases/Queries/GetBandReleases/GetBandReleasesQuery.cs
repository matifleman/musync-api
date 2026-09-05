using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Releases.Queries.GetBandReleases
{
    public sealed record GetBandReleasesQuery(int BandId, int PageNumber = 1, int PageSize = 20) : IRequest<List<ReleaseListItemDTO>>;
}
