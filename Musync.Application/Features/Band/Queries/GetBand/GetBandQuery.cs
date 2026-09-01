using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.GetBand
{
    public sealed record GetBandQuery(int BandId) : IRequest<BandDTO>;
}
