using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.User.Commands.CompleteOnboarding
{
    public sealed class CompleteOnboardingCommandHandler : IRequestHandler<CompleteOnboardingCommand, CurrentUserDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CompleteOnboardingCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        // Idempotent: completing twice is harmless, so a retried request after a flaky response
        // can't leave the account stuck in the flow.
        public async Task<CurrentUserDTO> Handle(CompleteOnboardingCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _currentUserService.GetCurrentUserAsync();

            if (!user.OnboardingCompleted)
            {
                user.OnboardingCompleted = true;
                await _userManager.UpdateAsync(user);
            }

            // The full user, so the app can use this response as its session user as-is.
            return _mapper.Map<CurrentUserDTO>(await _userManager.LoadSelfProfileAsync(user.Id, cancellationToken));
        }
    }
}
