using MediatR;

namespace Musync.Application.Features.Post.Commands.UpdatePostCaption
{
    public sealed record UpdatePostCaptionCommand(int PostId, string? Caption) : IRequest<PostDTO>;
}
