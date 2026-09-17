using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Song : BaseEntity
    {
        public required int ReleaseId { get; set; }
        public Release? Release { get; set; }
        public required string Title { get; set; }
        public int TrackNumber { get; set; }
    }
}
