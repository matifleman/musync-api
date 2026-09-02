namespace Musync.Application.DTOs
{
    public record BandMemberDTO
    {
        public int UserId { get; init; }
        public required string UserName { get; init; }
        public required string ProfilePicture { get; init; }
        public int InstrumentId { get; init; }
        public required string InstrumentName { get; init; }
    }
}
