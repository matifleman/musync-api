using FluentValidation.Results;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Like.Commands.LikePost
{
    public sealed class LikePostCommandHandler : IRequestHandler<LikePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostLikeRepository _postLikeRepository;

        public LikePostCommandHandler(IPostRepository postRepository, ICurrentUserService currentUserService, IPostLikeRepository postLikeRepository)
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
            _postLikeRepository = postLikeRepository;
        }
        public async Task<Unit> Handle(LikePostCommand request, CancellationToken cancellationToken)
        {
            LikePostCommandValidator validator = new LikePostCommandValidator(_postRepository);
            ValidationResult validationResult = await validator.ValidateAsync(request);
            if(validationResult.Errors.Any())
                throw new BadRequestException("Invalid like request", validationResult);

            int currentUserId = _currentUserService.CurrentUserId;

            bool isPostAlreadyLiked = await _postLikeRepository.HasUserLikedPost(currentUserId, request.postId);
            if (isPostAlreadyLiked) return Unit.Value;

            await _postLikeRepository.CreateAsync(new PostLike
            {
                PostId = request.postId,
                UserId = currentUserId
            });

            return Unit.Value;
        }
    }
}
