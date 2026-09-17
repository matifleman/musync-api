using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Musync.Application.Common;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Band.Releases.Commands.CreateRelease
{
    public sealed class CreateReleaseCommandHandler : IRequestHandler<CreateReleaseCommand, ReleaseDetailDTO>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBandRepository _bandRepository;
        private readonly IReleaseRepository _releaseRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateReleaseCommandHandler(
            IWebHostEnvironment env,
            IBandRepository bandRepository,
            IReleaseRepository releaseRepository,
            ICurrentUserService currentUserService)
        {
            _env = env;
            _bandRepository = bandRepository;
            _releaseRepository = releaseRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ReleaseDetailDTO> Handle(CreateReleaseCommand request, CancellationToken cancellationToken)
        {
            Domain.Band band = await _bandRepository.GetByIdAsync(request.BandId)
                ?? throw new NotFoundException($"Band with id '{request.BandId}' not found");

            if (band.CreatedById != _currentUserService.CurrentUserId)
                throw new BadRequestException("Only the band leader can post a release");

            CreateReleaseCommandValidator validator = new CreateReleaseCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid release", validationResult);

            Release release = new()
            {
                BandId = request.BandId,
                Title = request.Title,
                Type = request.Type,
                Cover = await SaveCover(request.Cover, cancellationToken),
                Songs = request.Songs
                    .Select((title, index) => new Song { ReleaseId = 0, Title = title, TrackNumber = index + 1 })
                    .ToList()
            };

            Release created = await _releaseRepository.CreateAsync(release);

            return ReleaseMapper.ToDetailDto(created);
        }

        private async Task<string> SaveCover(IFormFile cover, CancellationToken cancellationToken)
        {
            string fileName = ImageUploadValidator.GenerateSafeFileName(cover.FileName);
            string coversDirectory = Path.Combine(_env.WebRootPath, "release-covers");
            string savePath = Path.Combine(coversDirectory, fileName);

            if (!Directory.Exists(coversDirectory))
                Directory.CreateDirectory(coversDirectory);

            using var stream = new FileStream(savePath, FileMode.Create);
            await cover.CopyToAsync(stream, cancellationToken);

            return $"release-covers/{fileName}";
        }
    }
}
