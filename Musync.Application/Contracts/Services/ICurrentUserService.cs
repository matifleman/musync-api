using Musync.Domain;

namespace Musync.Application.Contracts.Services
{
    public interface ICurrentUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        int CurrentUserId { get; }
    }
}
