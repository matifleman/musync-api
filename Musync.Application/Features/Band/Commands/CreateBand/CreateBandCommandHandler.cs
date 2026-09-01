using FluentValidation.Results;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.CreateBand
{
    public sealed class CreateBandCommandHandler : IRequestHandler<CreateBandCommand, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IInstrumentRepository _instrumentRepository;

        public CreateBandCommandHandler(IBandRepository bandRepository, IInstrumentRepository instrumentRepository)
        {
            _bandRepository = bandRepository;
            _instrumentRepository = instrumentRepository;
        }

        public async Task<BandDTO> Handle(CreateBandCommand request, CancellationToken cancellationToken)
        {
            CreateBandCommandValidator validator = new CreateBandCommandValidator(_instrumentRepository);
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid band", validationResult);

            List<Domain.Instrument> requiredInstruments = await _instrumentRepository.GetByIdsAsync(request.InstrumentIds);

            Domain.Band band = new Domain.Band
            {
                Name = request.Name,
                RequiredInstruments = requiredInstruments
            };

            Domain.Band created = await _bandRepository.CreateAsync(band);
            created.Members = [];

            return BandMapper.ToBandDto(created);
        }
    }
}
