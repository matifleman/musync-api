using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Comment.Commands.CreateComment
{
    public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDTO>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(
            ICurrentUserService currentUserService,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentDTO> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            // Parent first: a bad postId is "not found", not "bad comment".
            _ = await _postRepository.GetByIdAsync(request.PostId)
                ?? throw new NotFoundException($"Post with ID {request.PostId} not found.");

            // Trim before validating, not after, so the length rule measures the stored value
            // and NotEmpty catches a comment that is nothing but whitespace.
            CreateCommentCommand trimmedRequest = request with { Text = request.Text?.Trim() };

            CreateCommentCommandValidator validator = new CreateCommentCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(trimmedRequest, cancellationToken);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid comment", validationResult);

            Domain.Comment comment = new Domain.Comment
            {
                PostId = request.PostId,
                // Never from the body: the author is whoever holds the token.
                AuthorId = _currentUserService.CurrentUserId,
                Text = trimmedRequest.Text!
            };

            Domain.Comment createdComment = await _commentRepository.CreateAsync(comment);

            // Re-read with the author for the response DTO; the copy above never carried one.
            Domain.Comment commentWithAuthor = await _commentRepository.GetCommentWithAuthorAsync(createdComment.Id)
                ?? throw new NotFoundException($"Comment with ID {createdComment.Id} not found.");

            return _mapper.Map<CommentDTO>(commentWithAuthor);
        }
    }
}
