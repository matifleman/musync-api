using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class BandFollower : BaseEntity
    {
        public required int BandId { get; set; }
        public required int UserId { get; set; }

        public Band? Band { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
