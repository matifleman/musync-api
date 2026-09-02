namespace Musync.Application.DTOs
{
    public record BandFollowResultDTO
    {
        public int BandId { get; init; }
        public bool IsFollowing { get; init; }
        public int FollowersCount { get; init; }
    }
}
