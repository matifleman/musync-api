namespace Musync.Application.Models.Identity
{
    public record LoginRequest(
        string UserName,
        string email,
        string Password
    );
}
