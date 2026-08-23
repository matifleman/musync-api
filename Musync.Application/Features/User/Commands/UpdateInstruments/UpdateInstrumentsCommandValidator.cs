using FluentValidation;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.User.Commands.UpdateInstruments
{
    public sealed class UpdateInstrumentsCommandValidator : AbstractValidator<UpdateInstrumentsCommand>
    {
        private readonly IInstrumentRepository _instrumentRepository;

        public UpdateInstrumentsCommandValidator(IInstrumentRepository instrumentRepository)
        {
            _instrumentRepository = instrumentRepository;

            RuleFor(c => c.InstrumentIds)
                .NotNull()
                .WithMessage("InstrumentIds is required.");

            RuleFor(c => c.InstrumentIds)
                .Must(ids => ids.Count <= 2)
                .WithMessage("You can select at most 2 instruments.")
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate instrument ids are not allowed.")
                .MustAsync(AllInstrumentsExist)
                .WithMessage("One or more instruments do not exist.")
                .When(c => c.InstrumentIds is not null);
        }

        private async Task<bool> AllInstrumentsExist(List<int> ids, CancellationToken cancellationToken)
        {
            if (ids.Count == 0)
                return true;

            List<Domain.Instrument> foundInstruments = await _instrumentRepository.GetByIdsAsync(ids);
            return foundInstruments.Count == ids.Distinct().Count();
        }
    }
}
