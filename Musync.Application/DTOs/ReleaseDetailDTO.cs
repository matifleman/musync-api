using Musync.Domain;

namespace Musync.Application.DTOs
{
    public record ReleaseDetailDTO
    {
        public required int Id { get; init; }
        public required int BandId { get; init; }
        public required string Title { get; init; }
        public required ReleaseType Type { get; init; }
        public required string Cover { get; init; }
        public DateTimeOffset? CreatedAt { get; init; }
        public List<SongDTO> Songs { get; init; } = [];
    }
}
