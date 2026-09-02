using MediatR;

namespace Musync.Application.Features.Genre.Queries
{
    public sealed class GetGenresQuery : IRequest<List<GenreDTO>>
    {
    }
}
