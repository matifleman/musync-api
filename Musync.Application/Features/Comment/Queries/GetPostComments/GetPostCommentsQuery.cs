using MediatR;

namespace Musync.Application.Features.Comment.Queries.GetPostComments
{
    public sealed record GetPostCommentsQuery(int PostId, int PageNumber = 1, int PageSize = 20) : IRequest<List<CommentDTO>>;
}
