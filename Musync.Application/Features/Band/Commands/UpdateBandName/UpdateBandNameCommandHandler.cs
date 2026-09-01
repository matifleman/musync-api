using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using FluentValidation.Results;

namespace Musync.Application.Features.Band.Commands.UpdateBandName
{
    public sealed class UpdateBandNameCommandHandler : IRequestHandler<UpdateBandNameCommand, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBandNameCommandHandler(IBandRepository bandRepository, ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(UpdateBandNameCommand request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with ID {request.BandId} not found.");
            
            if (band.CreatedById != _currentUserService.CurrentUserId)
                throw new BadRequestException("Only the band leader can edit this band.");

            UpdateBandNameCommandValidator validator = new UpdateBandNameCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if(validationResult.Errors.Any())
                throw new BadRequestException("Invalid band name", validationResult);

            band.Name = request.Name;
            await _bandRepository.UpdateAsync(band);
        
            return BandMapper.ToBandDto(band);
        }
    }
}