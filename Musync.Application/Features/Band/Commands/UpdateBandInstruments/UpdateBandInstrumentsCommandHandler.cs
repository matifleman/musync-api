using FluentValidation.Results;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.UpdateBandInstruments
{
    public sealed class UpdateBandInstrumentsCommandHandler : IRequestHandler<UpdateBandInstrumentsCommand, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IBandMemberRepository _bandMemberRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBandInstrumentsCommandHandler(
            IBandRepository bandRepository,
            IBandMemberRepository bandMemberRepository,
            IInstrumentRepository instrumentRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _bandMemberRepository = bandMemberRepository;
            _instrumentRepository = instrumentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(UpdateBandInstrumentsCommand request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            if (band.CreatedById != _currentUserService.CurrentUserId)
                throw new BadRequestException("Only the band leader can edit this band");

            UpdateBandInstrumentsCommandValidator validator = new UpdateBandInstrumentsCommandValidator(_instrumentRepository);
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid instruments", validationResult);

            List<int> removedInstrumentIds = band.RequiredInstruments
                .Select(i => i.Id)
                .Except(request.InstrumentIds)
                .ToList();

            foreach (int removedInstrumentId in removedInstrumentIds)
            {
                bool isTaken = await _bandMemberRepository.IsInstrumentTakenAsync(request.BandId, removedInstrumentId);
                if (isTaken)
                    throw new BadRequestException("Cannot remove an instrument that a member is currently playing");
            }

            List<Domain.Instrument> newInstruments = await _instrumentRepository.GetByIdsAsync(request.InstrumentIds);
            band.RequiredInstruments = newInstruments;
            await _bandRepository.UpdateAsync(band);

            return BandMapper.ToBandDto(band);
        }
    }
}
