namespace Musync.Application.Models.Identity
{
    public record RefreshRequest(int UserId,string RefreshToken);
}
