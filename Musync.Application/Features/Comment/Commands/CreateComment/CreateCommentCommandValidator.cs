using FluentValidation;

namespace Musync.Application.Features.Comment.Commands.CreateComment
{
    public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            // Runs against the already-trimmed text (see the handler), so NotEmpty rejects a
            // whitespace-only comment and the length check measures what actually gets stored.
            RuleFor(command => command.Text)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Comment text is required.")
                .MaximumLength(Domain.Comment.TextMaxLength)
                .WithMessage($"Comment must be {Domain.Comment.TextMaxLength} characters or fewer.");
        }
    }
}
