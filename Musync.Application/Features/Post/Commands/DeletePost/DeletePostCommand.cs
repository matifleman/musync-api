using MediatR;

namespace Musync.Application.Features.Post.Commands.DeletePost
{
    public sealed record DeletePostCommand(int PostId) : IRequest<Unit>;
}
