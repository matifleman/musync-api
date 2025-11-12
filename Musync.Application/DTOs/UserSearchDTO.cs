namespace Musync.Application.DTOs
{
    public class UserSearchDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public int FollowersCount { get; set; }
        public bool IsFollowed { get; set; }
    }
}
