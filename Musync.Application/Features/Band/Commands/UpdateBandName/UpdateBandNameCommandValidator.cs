using FluentValidation;

namespace Musync.Application.Features.Band.Commands.UpdateBandName
{
    public sealed class UpdateBandNameCommandValidator : AbstractValidator<UpdateBandNameCommand>
    {
        public UpdateBandNameCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be 100 characters or fewer.");
        }
    }
}