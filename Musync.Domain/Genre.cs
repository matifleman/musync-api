using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Genre : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<Band> Bands { get; set; } = [];
        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
