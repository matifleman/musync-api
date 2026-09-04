using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.User.Commands.UpdateGenres
{
    public sealed class UpdateGenresCommandHandler : IRequestHandler<UpdateGenresCommand, CurrentUserDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public UpdateGenresCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IGenreRepository genreRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<CurrentUserDTO> Handle(UpdateGenresCommand request, CancellationToken cancellationToken)
        {
            UpdateGenresCommandValidator validator = new UpdateGenresCommandValidator(_genreRepository);
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid genres", validationResult);

            ApplicationUser currentUser = (await _currentUserService.GetCurrentUserAsync())!;

            ApplicationUser user = await _userManager.Users
                .Include(u => u.FavoriteGenres)
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken)
                ?? throw new NotFoundException("Current user not found");

            List<Domain.Genre> selectedGenres = await _genreRepository.GetByIdsAsync(request.GenreIds);

            user.FavoriteGenres = selectedGenres;

            await _userManager.UpdateAsync(user);

            return _mapper.Map<CurrentUserDTO>(user);
        }
    }
}
