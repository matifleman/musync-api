using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Releases.Queries.GetReleaseById
{
    public sealed record GetReleaseByIdQuery(int ReleaseId) : IRequest<ReleaseDetailDTO>;
}
