using FluentValidation;

namespace Musync.Application.Features.Post.Commands.UpdatePostCaption
{
    public sealed class UpdatePostCaptionCommandValidator : AbstractValidator<UpdatePostCaptionCommand>
    {
        public UpdatePostCaptionCommandValidator()
        {
            // Length only - a null or empty caption is a legitimate "clear the caption".
            // MaximumLength passes on null, which is exactly what we want here.
            RuleFor(command => command.Caption)
                .MaximumLength(Domain.Post.CaptionMaxLength)
                .WithMessage($"Caption must be {Domain.Post.CaptionMaxLength} characters or fewer.");
        }
    }
}
