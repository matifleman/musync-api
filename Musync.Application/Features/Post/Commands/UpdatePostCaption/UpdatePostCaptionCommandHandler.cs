using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Post.Commands.UpdatePostCaption
{
    public sealed class UpdatePostCaptionCommandHandler : IRequestHandler<UpdatePostCaptionCommand, PostDTO>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IMapper _mapper;

        public UpdatePostCaptionCommandHandler(
            ICurrentUserService currentUserService,
            IPostRepository postRepository,
            IPostLikeRepository postLikeRepository,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _mapper = mapper;
        }

        public async Task<PostDTO> Handle(UpdatePostCaptionCommand request, CancellationToken cancellationToken)
        {
            int currentUserId = _currentUserService.CurrentUserId;

            // Deliberately the author-less read: UpdateAsync marks everything reachable from the
            // entity as Modified, so an Included ApplicationUser would be rewritten too - and its
            // ConcurrencyStamp is a concurrency token, so a parallel profile edit would 500 here.
            Domain.Post post = await _postRepository.GetByIdAsync(request.PostId)
                ?? throw new NotFoundException($"Post with ID {request.PostId} not found.");

            // 404 before 403, so a wrong id never comes back as "you're not allowed".
            if (post.AuthorId != currentUserId)
                throw new ForbiddenException("Only the author can edit this post.");

            UpdatePostCaptionCommandValidator validator = new UpdatePostCaptionCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid caption", validationResult);

            post.Caption = string.IsNullOrWhiteSpace(request.Caption) ? null : request.Caption.Trim();
            await _postRepository.UpdateAsync(post);

            // Re-read with the author for the response DTO; the copy above never carried one.
            Domain.Post updatedPost = await _postRepository.GetPostWithAuthorAsync(request.PostId)
                ?? throw new NotFoundException($"Post with ID {request.PostId} not found.");

            PostDTO postDTO = _mapper.Map<PostDTO>(updatedPost);
            postDTO.Liked = await _postLikeRepository.HasUserLikedPost(currentUserId, updatedPost.Id);

            return postDTO;
        }
    }
}
