using Musync.Domain;

namespace Musync.Application.DTOs
{
    public record ReleaseListItemDTO
    {
        public required int Id { get; init; }
        public required string Title { get; init; }
        public required ReleaseType Type { get; init; }
        public required string Cover { get; init; }
        public DateTimeOffset? CreatedAt { get; init; }
    }
}
