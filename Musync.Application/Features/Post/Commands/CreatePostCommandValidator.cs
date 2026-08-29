using FluentValidation;
using Musync.Application.Common;

namespace Musync.Application.Features.Post.Commands
{
    public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.Image)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Image is required.")
                .Must(image => image.Length > 0 && image.Length <= ImageUploadValidator.MaxFileSizeBytes)
                .WithMessage($"Image must be {ImageUploadValidator.MaxFileSizeBytes / (1024 * 1024)}MB or smaller.")
                .Must(image => ImageUploadValidator.HasAllowedExtension(image.FileName))
                .WithMessage("Image must be a .jpg, .jpeg, .png, .gif, or .webp file.")
                .MustAsync((image, cancellationToken) => ImageUploadValidator.HasValidImageSignatureAsync(image, cancellationToken))
                .WithMessage("Image file content does not match its extension.");
        }
    }
}
