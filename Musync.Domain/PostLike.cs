using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class PostLike : BaseEntity
    {
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
