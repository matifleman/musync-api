using FluentValidation;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.Band.Commands.CreateBand
{
    public sealed class CreateBandCommandValidator : AbstractValidator<CreateBandCommand>
    {
        private readonly IInstrumentRepository _instrumentRepository;

        public CreateBandCommandValidator(IInstrumentRepository instrumentRepository)
        {
            _instrumentRepository = instrumentRepository;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be 100 characters or fewer.");

            RuleFor(c => c.InstrumentIds)
                .NotNull().WithMessage("InstrumentIds is required.")
                .Must(ids => ids.Count > 0).WithMessage("At least one instrument is required.")
                .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Duplicate instrument ids are not allowed.")
                .MustAsync(AllInstrumentsExist).WithMessage("One or more instruments do not exist.")
                .When(c => c.InstrumentIds is not null);
        }

        private async Task<bool> AllInstrumentsExist(List<int> ids, CancellationToken cancellationToken)
        {
            if (ids.Count == 0)
                return true;

            List<Domain.Instrument> found = await _instrumentRepository.GetByIdsAsync(ids);
            return found.Count == ids.Distinct().Count();
        }
    }
}
