using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Instrument : BaseEntity
    {
        public required string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
