using MediatR;

namespace Musync.Application.Features.Post.Queries.GetUserPosts
{
    public sealed record GetUserPostsQuery(int authorId) : IRequest<List<PostDTO>>;
}
