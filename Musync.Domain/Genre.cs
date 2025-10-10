using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Genre : BaseEntity
    {
        public required string Name { get; set; }
        public List<Band> Bands { get; set; } = [];
    }
}
