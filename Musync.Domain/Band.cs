using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Band : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Genre> Genres { get; set; } = [];
    }
}
