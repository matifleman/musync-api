using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Comment.Commands.DeleteComment
{
    public sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public DeleteCommentCommandHandler(
            ICurrentUserService currentUserService,
            ICommentRepository commentRepository,
            IPostRepository postRepository)
        {
            _currentUserService = currentUserService;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        // No validator: the only input is the route id, and existence/ownership are 404/403
        // concerns the handler checks itself - same shape as DeletePostCommandHandler.
        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            Domain.Comment comment = await _commentRepository.GetByIdAsync(request.CommentId)
                ?? throw new NotFoundException($"Comment with ID {request.CommentId} not found.");

            Domain.Post post = await _postRepository.GetByIdAsync(comment.PostId)
                ?? throw new NotFoundException($"Post with ID {comment.PostId} not found.");

            int currentUserId = _currentUserService.CurrentUserId;

            // Moderation on your own post: the post's author can remove anyone's comment on it,
            // and a commenter can always remove their own. 404 before 403, so a wrong id never
            // comes back as "you're not allowed".
            if (comment.AuthorId != currentUserId && post.AuthorId != currentUserId)
                throw new ForbiddenException("Only the comment's author or the post's author can delete this comment.");

            await _commentRepository.DeleteAsync(comment);

            return Unit.Value;
        }
    }
}
