using FluentValidation;
using Musync.Application.Contracts.Persistance;

namespace Musync.Application.Features.Band.Commands.UpdateBandGenres
{
    public sealed class UpdateBandGenresCommandValidator : AbstractValidator<UpdateBandGenresCommand>
    {
        private readonly IGenreRepository _genreRepository;

        public UpdateBandGenresCommandValidator(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;

            RuleFor(c => c.GenreIds)
                .NotNull().WithMessage("GenreIds is required.")
                .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Duplicate genre ids are not allowed.")
                .MustAsync(AllGenresExist).WithMessage("One or more genres do not exist.")
                .When(c => c.GenreIds is not null);
        }

        private async Task<bool> AllGenresExist(List<int> ids, CancellationToken cancellationToken)
        {
            if (ids.Count == 0)
                return true;

            List<Domain.Genre> found = await _genreRepository.GetByIdsAsync(ids);
            return found.Count == ids.Distinct().Count();
        }
    }
}
