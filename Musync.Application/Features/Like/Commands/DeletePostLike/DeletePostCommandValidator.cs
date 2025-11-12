using FluentValidation;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.Like.Commands.DeletePostLike
{
    public sealed class DeletePostCommandValidator : AbstractValidator<DeletePostLikeCommand>
    {
        private readonly IPostRepository _postRepository;

        public DeletePostCommandValidator(IPostRepository postRepository)
        {
            _postRepository = postRepository;
            
            RuleFor(command => command.postId)
                .NotNull()
                .WithMessage("PostId is required.")
                .MustAsync(PostMustExist)
                .WithMessage("Post does not exist.");
        }

        private async Task<bool> PostMustExist(int postId, CancellationToken cancellationToken)
        {
            Domain.Post? post = await _postRepository.GetByIdAsync(postId);
            return post is not null;
        }
    }
}
