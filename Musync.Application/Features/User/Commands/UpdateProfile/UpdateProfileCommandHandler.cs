using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.User.Commands.UpdateProfile
{
    public sealed class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, CurrentUserDTO>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateProfileCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<CurrentUserDTO> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            UpdateProfileCommandValidator validator = new UpdateProfileCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid profile data", validationResult);

            ApplicationUser user = await _currentUserService.GetCurrentUserAsync();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            if (!string.Equals(user.UserName, request.UserName, StringComparison.Ordinal))
            {
                IdentityResult result = await _userManager.SetUserNameAsync(user, request.UserName);
                if (!result.Succeeded)
                    throw new BadRequestException("Invalid username", result.Errors);
            }
            else
            {
                await _userManager.UpdateAsync(user);
            }

            return _mapper.Map<CurrentUserDTO>(user);
        }
    }
}
