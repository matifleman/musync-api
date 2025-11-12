using MediatR;

namespace Musync.Application.Features.Like.Commands.DeletePostLike
{
    public sealed record DeletePostLikeCommand(int postId) : IRequest<Unit>;
}
