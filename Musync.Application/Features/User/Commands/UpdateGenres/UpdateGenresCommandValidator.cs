using FluentValidation;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.User.Commands.UpdateGenres
{
    public sealed class UpdateGenresCommandValidator : AbstractValidator<UpdateGenresCommand>
    {
        private readonly IGenreRepository _genreRepository;

        public UpdateGenresCommandValidator(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;

            RuleFor(c => c.GenreIds)
                .NotNull()
                .WithMessage("GenreIds is required.");

            RuleFor(c => c.GenreIds)
                .Must(ids => ids.Count <= 2)
                .WithMessage("You can select at most 2 genres.")
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate genre ids are not allowed.")
                .MustAsync(AllGenresExist)
                .WithMessage("One or more genres do not exist.")
                .When(c => c.GenreIds is not null);
        }

        private async Task<bool> AllGenresExist(List<int> ids, CancellationToken cancellationToken)
        {
            if (ids.Count == 0)
                return true;

            List<Domain.Genre> foundGenres = await _genreRepository.GetByIdsAsync(ids);
            return foundGenres.Count == ids.Distinct().Count();
        }
    }
}
