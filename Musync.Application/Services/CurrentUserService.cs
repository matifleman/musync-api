using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Musync.Application.Contracts.Services;
using Musync.Domain;

namespace Musync.Application.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public int CurrentUserId => int.Parse(_userManager.GetUserId(_httpContextAccessor.HttpContext?.User));

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            int userId = int.Parse(_userManager.GetUserId(_httpContextAccessor.HttpContext?.User));

            ApplicationUser? user = await _userManager.Users
                .Include(u => u.Followed)
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user is null)
                throw new Exception("Current user not found");

            return user;
        }
    }
}
