using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Musync.Domain;

namespace Musync.Application.Features.Post.Commands
{
    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreatePostCommandValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            
            RuleFor(p => p.Image)
                .NotEmpty()
                .WithMessage("Image is required.");

            RuleFor(p => p.AuthorId)
                .NotEmpty()
                .WithMessage("AuthorId is required.")
                .MustAsync(AuthorMustExist)
                .WithMessage("Author does not exist.");
        }

        private async Task<bool> AuthorMustExist(int authorId, CancellationToken cancellationToken)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(authorId.ToString());
            return user is not null;
        }
    }
}
