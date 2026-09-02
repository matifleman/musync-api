using FluentValidation.Results;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.UpdateBandGenres
{
    public sealed class UpdateBandGenresCommandHandler : IRequestHandler<UpdateBandGenresCommand, BandDTO>
    {
        private readonly IBandRepository _bandRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBandGenresCommandHandler(
            IBandRepository bandRepository,
            IGenreRepository genreRepository,
            ICurrentUserService currentUserService)
        {
            _bandRepository = bandRepository;
            _genreRepository = genreRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(UpdateBandGenresCommand request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            if (band.CreatedById != _currentUserService.CurrentUserId)
                throw new BadRequestException("Only the band leader can edit this band");

            UpdateBandGenresCommandValidator validator = new UpdateBandGenresCommandValidator(_genreRepository);
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid genres", validationResult);

            List<Domain.Genre> genres = await _genreRepository.GetByIdsAsync(request.GenreIds);
            band.Genres = genres;
            await _bandRepository.UpdateAsync(band);

            return BandMapper.ToBandDto(band);
        }
    }
}
