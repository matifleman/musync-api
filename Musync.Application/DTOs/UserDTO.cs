namespace Musync.Application.DTOs
{
    public record UserDTO
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string UserName { get; init; }
        public int Age { get; init; }
        public required string Email { get; init; }
        public required string ProfilePicture { get; init; }
    };
}
