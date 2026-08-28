using FluentValidation;
using Musync.Application.Common;

namespace Musync.Application.Features.User.Commands.UpdateAvatar
{
    public sealed class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
    {
        public UpdateAvatarCommandValidator()
        {
            RuleFor(c => c.newAvatar)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Avatar image is required.")
                .Must(image => image.Length > 0 && image.Length <= ImageUploadValidator.MaxFileSizeBytes)
                .WithMessage($"Avatar image must be {ImageUploadValidator.MaxFileSizeBytes / (1024 * 1024)}MB or smaller.")
                .Must(image => ImageUploadValidator.HasAllowedExtension(image.FileName))
                .WithMessage("Avatar must be a .jpg, .jpeg, .png, .gif, or .webp file.")
                .MustAsync((image, cancellationToken) => ImageUploadValidator.HasValidImageSignatureAsync(image, cancellationToken))
                .WithMessage("Avatar file content does not match its extension.");
        }
    }
}
