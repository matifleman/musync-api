using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Band : BaseEntity
    {
        public required string Name { get; set; }
        public List<Genre> Genres { get; set; } = [];
    }
}
