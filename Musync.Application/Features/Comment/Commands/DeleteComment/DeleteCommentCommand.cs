using MediatR;

namespace Musync.Application.Features.Comment.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(int CommentId) : IRequest<Unit>;
}
