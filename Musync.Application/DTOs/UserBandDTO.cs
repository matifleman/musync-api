namespace Musync.Application.DTOs
{
    public record UserBandDTO
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public string? ProfilePicture { get; init; }
        public bool IsLeader { get; init; }
        public int? InstrumentId { get; init; }
        public string? InstrumentName { get; init; }
    }
}
