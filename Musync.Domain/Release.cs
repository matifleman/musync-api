using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Release : BaseEntity
    {
        public required int BandId { get; set; }
        public Band? Band { get; set; }
        public required string Title { get; set; }
        public required ReleaseType Type { get; set; }
        public required string Cover { get; set; }
        public ICollection<Song> Songs { get; set; } = [];
    }
}
