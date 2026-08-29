using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class RefreshToken : BaseEntity
    {
        public required string TokenHash { get; set; }
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
    }
}
