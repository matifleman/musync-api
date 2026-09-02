using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.UpdateBandGenres
{
    public sealed record UpdateBandGenresCommand(int BandId, List<int> GenreIds) : IRequest<BandDTO>;
}
