using MediatR;

namespace Musync.Application.Features.Post.Queries.GetFeed
{
    public sealed record GetFeedQuery(int PageNumber = 1, int PageSize = 20) : IRequest<List<PostDTO>>;
}
