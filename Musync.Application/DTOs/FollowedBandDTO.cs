namespace Musync.Application.DTOs
{
    public record FollowedBandDTO
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public string? ProfilePicture { get; init; }
        public int MemberCount { get; init; }
    }
}
