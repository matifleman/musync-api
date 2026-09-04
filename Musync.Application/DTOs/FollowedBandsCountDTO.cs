namespace Musync.Application.DTOs
{
    public record FollowedBandsCountDTO
    {
        public required int UserId { get; init; }
        public required int FollowedBandsCount { get; init; }
    }
}
