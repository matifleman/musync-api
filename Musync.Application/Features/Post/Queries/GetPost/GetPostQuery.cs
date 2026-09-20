using MediatR;

namespace Musync.Application.Features.Post.Queries.GetPost
{
    public sealed record GetPostQuery(int PostId) : IRequest<PostDTO>;
}
