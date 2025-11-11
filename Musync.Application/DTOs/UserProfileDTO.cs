namespace Musync.Application.DTOs
{
    public class UserProfileDTO
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string ProfilePicture { get; init; }
        public int FollowersCount { get; init; }
        public int FollowedCount { get; init; }
        public bool IsFollowed { get; set; }
    }
}
