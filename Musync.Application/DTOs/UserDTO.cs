namespace Musync.Application.DTOs
{
    public record UserDTO
    {
        public int Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string UserName { get; init; }
        public int Age { get; init; }
        public required string Email { get; init; }
        public required string ProfilePicture { get; init; }
        public int FollowersCount { get; init; }
        public int FollowedCount { get; init; }
        public bool IsFollowed { get; set; }
    };
}
