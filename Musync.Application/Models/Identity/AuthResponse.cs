using Musync.Application.DTOs;

namespace Musync.Application.Models.Identity
{
    public record AuthResponse(
        CurrentUserDTO User,
        string AccessToken,
        string RefreshToken
    );
}
