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

namespace Musync.Application.Features.User.Commands.UpdateInstruments
{
    public sealed class UpdateInstrumentsCommandHandler : IRequestHandler<UpdateInstrumentsCommand, CurrentUserDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IMapper _mapper;

        public UpdateInstrumentsCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IInstrumentRepository instrumentRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _instrumentRepository = instrumentRepository;
            _mapper = mapper;
        }

        public async Task<CurrentUserDTO> Handle(UpdateInstrumentsCommand request, CancellationToken cancellationToken)
        {
            UpdateInstrumentsCommandValidator validator = new UpdateInstrumentsCommandValidator(_instrumentRepository);
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid instruments", validationResult);

            ApplicationUser currentUser = (await _currentUserService.GetCurrentUserAsync())!;

            ApplicationUser user = await _userManager.Users
                .Include(u => u.FavoriteInstruments)
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken)
                ?? throw new NotFoundException("Current user not found");

            List<Domain.Instrument> selectedInstruments = await _instrumentRepository.GetByIdsAsync(request.InstrumentIds);

            user.FavoriteInstruments = selectedInstruments;

            await _userManager.UpdateAsync(user);

            return _mapper.Map<CurrentUserDTO>(user);
        }
    }
}
