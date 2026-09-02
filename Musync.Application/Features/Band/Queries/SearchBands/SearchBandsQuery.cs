using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Queries.SearchBands
{
    public sealed record SearchBandsQuery(
        string? SearchTerm,
        int? InstrumentId,
        int? GenreId,
        int PageNumber = 1,
        int PageSize = 20) : IRequest<List<BandSearchDTO>>;
}
