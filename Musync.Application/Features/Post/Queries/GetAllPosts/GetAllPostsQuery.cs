using MediatR;

namespace Musync.Application.Features.Post.Queries.GetAllPosts
{
    public sealed record GetAllPostsQuery : IRequest<List<PostDTO>>;
}
