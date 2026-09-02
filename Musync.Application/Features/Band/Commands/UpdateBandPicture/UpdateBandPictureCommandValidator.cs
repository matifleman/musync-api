using FluentValidation;
using Musync.Application.Common;

namespace Musync.Application.Features.Band.Commands.UpdateBandPicture
{
    public sealed class UpdateBandPictureCommandValidator : AbstractValidator<UpdateBandPictureCommand>
    {
        public UpdateBandPictureCommandValidator()
        {
            RuleFor(c => c.Picture)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Picture is required.")
                .Must(image => image.Length > 0 && image.Length <= ImageUploadValidator.MaxFileSizeBytes)
                .WithMessage($"Band picture must be {ImageUploadValidator.MaxFileSizeBytes / (1024 * 1024)}MB or smaller.")
                .Must(image => ImageUploadValidator.HasAllowedExtension(image.FileName))
                .WithMessage("Band picture must be a .jpg, .jpeg, .png, .gif, or .webp file.")
                .MustAsync((image, cancellationToken) => ImageUploadValidator.HasValidImageSignatureAsync(image, cancellationToken))
                .WithMessage("Band picture file content does not match its extension.");

        }
    }
}