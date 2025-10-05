using Musync.Application.Models.Identity;

namespace Musync.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginRequest request);
        Task<AuthResponse> Register(RegistrationRequest request);
    }
}
