using MediatR;

namespace Musync.Application.Features.Like.Commands.LikePost
{
    public sealed record LikePostCommand(int postId) : IRequest<Unit>;
}
