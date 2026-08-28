using FluentValidation;

namespace Musync.Application.Features.Post.Commands
{
    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.Image)
                .NotEmpty()
                .WithMessage("Image is required.");
        }
    }
}
