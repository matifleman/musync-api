using MediatR;

namespace Musync.Application.Features.Comment.Commands.CreateComment
{
    // Text is nullable so a missing body field reaches the validator as "required"
    // rather than being rejected by model binding with a shape the client can't read.
    public sealed record CreateCommentCommand(int PostId, string? Text) : IRequest<CommentDTO>;
}
