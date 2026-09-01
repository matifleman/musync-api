using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.SearchBands
{
    public sealed record SearchBandsQuery(string SearchTerm, int PageNumber = 1, int PageSize = 20)
        : IRequest<List<BandSearchDTO>>;
}
