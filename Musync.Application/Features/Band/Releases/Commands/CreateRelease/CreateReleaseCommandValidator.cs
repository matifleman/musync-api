using FluentValidation;
using Musync.Application.Common;
using Musync.Domain;

namespace Musync.Application.Features.Band.Releases.Commands.CreateRelease
{
    public sealed class CreateReleaseCommandValidator : AbstractValidator<CreateReleaseCommand>
    {
        public CreateReleaseCommandValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must be 100 characters or fewer.");

            RuleFor(c => c.Cover)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Cover is required.")
                .Must(image => image.Length > 0 && image.Length <= ImageUploadValidator.MaxFileSizeBytes)
                .WithMessage($"Cover must be {ImageUploadValidator.MaxFileSizeBytes / (1024 * 1024)}MB or smaller.")
                .Must(image => ImageUploadValidator.HasAllowedExtension(image.FileName))
                .WithMessage("Cover must be a .jpg, .jpeg, .png, .gif, or .webp file.")
                .MustAsync((image, cancellationToken) => ImageUploadValidator.HasValidImageSignatureAsync(image, cancellationToken))
                .WithMessage("Cover file content does not match its extension.");

            RuleFor(c => c.Songs)
                .NotNull().WithMessage("Songs is required.")
                .Must(songs => songs.Count > 0).WithMessage("At least one song is required.")
                .Must(songs => songs.All(s => !string.IsNullOrWhiteSpace(s))).WithMessage("Song titles cannot be empty.")
                .When(c => c.Songs is not null);

            RuleFor(c => c.Songs)
                .Must(songs => songs.Count == 1).WithMessage("A Single must have exactly one song.")
                .When(c => c.Songs is not null && c.Type == ReleaseType.Single);
        }
    }
}
