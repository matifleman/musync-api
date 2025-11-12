using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Services;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.User.Commands.UpdateAvatar
{
    public sealed class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, UserDTO>
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateAvatarCommandHandler(
            IWebHostEnvironment env, 
            ICurrentUserService currentUserService, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager)
        {
            _env = env;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<UserDTO> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            string avatarPath = await SaveImage(request.newAvatar, cancellationToken);
            ApplicationUser user = await _currentUserService.GetCurrentUserAsync();

            user.ProfilePicture = avatarPath;
            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserDTO>(user);
        }

        private async Task<string> SaveImage(IFormFile image, CancellationToken cancellationToken)
        {
            string imagePath = string.Empty;

            string fileName = $"{Guid.NewGuid()}_{image.FileName}";
            string imagesDirectory = Path.Combine(_env.WebRootPath, "profile-pictures");
            string savePath = Path.Combine(imagesDirectory, fileName);

            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            using var stream = new FileStream(savePath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            imagePath = $"profile-pictures/{fileName}";

            return imagePath;
        }
    }
}
