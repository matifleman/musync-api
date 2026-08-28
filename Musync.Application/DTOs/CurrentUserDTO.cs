namespace Musync.Application.DTOs
{
    public record CurrentUserDTO : UserDTO
    {
        public required string Email { get; init; }
    };
}
