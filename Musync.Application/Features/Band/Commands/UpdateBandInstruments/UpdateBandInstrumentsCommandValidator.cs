using FluentValidation;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.Band.Commands.UpdateBandInstruments
{
    public sealed class UpdateBandInstrumentsCommandValidator : AbstractValidator<UpdateBandInstrumentsCommand>
    {
        private readonly IInstrumentRepository _instrumentRepository;

        public UpdateBandInstrumentsCommandValidator(IInstrumentRepository instrumentRepository)
        {
            _instrumentRepository = instrumentRepository;

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
