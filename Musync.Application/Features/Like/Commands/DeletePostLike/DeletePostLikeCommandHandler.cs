using FluentValidation.Results;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Like.Commands.DeletePostLike
{
    public sealed class DeletePostLikeCommandHandler : IRequestHandler<DeletePostLikeCommand, Unit>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostLikeRepository _postLikeRepository;

        public DeletePostLikeCommandHandler(IPostRepository postRepository, ICurrentUserService currentUserService, IPostLikeRepository postLikeRepository)
        {
            _postRepository = postRepository;
            _currentUserService = currentUserService;
            _postLikeRepository = postLikeRepository;
        }
        public async Task<Unit> Handle(DeletePostLikeCommand request, CancellationToken cancellationToken)
        {
            DeletePostCommandValidator validator = new DeletePostCommandValidator(_postRepository);
            ValidationResult validationResult = await validator.ValidateAsync(request);
            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid like request", validationResult);

            int currentUserId = _currentUserService.CurrentUserId;

            Domain.PostLike? postLike = await _postLikeRepository.GetLikeOfUser(currentUserId, request.postId);
            if (postLike is null) return Unit.Value;

            await _postLikeRepository.DeleteAsync(postLike);

            return Unit.Value;
        }
    }
}
