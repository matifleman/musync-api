using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Musync.Application.Common;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Band.Commands.UpdateBandPicture
{
    public sealed class UpdateBandPictureCommandHandler : IRequestHandler<UpdateBandPictureCommand, BandDTO>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBandRepository _bandRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBandPictureCommandHandler(
            IWebHostEnvironment env,
            IBandRepository bandRepository,
            ICurrentUserService currentUserService)
        {
            _env = env;
            _bandRepository = bandRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BandDTO> Handle(UpdateBandPictureCommand request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetBandWithDetailsAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            if (band.CreatedById != _currentUserService.CurrentUserId)
                throw new BadRequestException("Only the band leader can edit this band");

            UpdateBandPictureCommandValidator validator = new UpdateBandPictureCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid band picture", validationResult);

            band.ProfilePicture = await SaveImage(request.Picture, cancellationToken);
            await _bandRepository.UpdateAsync(band);

            return BandMapper.ToBandDto(band);
        }

        private async Task<string> SaveImage(IFormFile image, CancellationToken cancellationToken)
        {
            string fileName = ImageUploadValidator.GenerateSafeFileName(image.FileName);
            string imagesDirectory = Path.Combine(_env.WebRootPath, "band-pictures");
            string savePath = Path.Combine(imagesDirectory, fileName);

            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            using var stream = new FileStream(savePath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            return $"band-pictures/{fileName}";
        }
    }
}
