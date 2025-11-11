using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Musync.Domain;

namespace Musync.Application.Features.Post.Queries.GetUserPosts
{
    public sealed class GetUserPostsQueryValidator : AbstractValidator<GetUserPostsQuery>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserPostsQueryValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            
            RuleFor(p => p.authorId)
                .NotNull()
                .WithMessage("Author ID is required")
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
