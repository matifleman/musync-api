using Musync.Application.DTOs;

namespace Musync.Application.Models.Identity
{
    public record AuthResponse(
        UserDTO User,
        string AccessToken,
        string RefreshToken
    );
}
