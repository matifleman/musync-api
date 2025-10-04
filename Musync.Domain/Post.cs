using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Post : BaseEntity
    {
        public int AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public string? Caption;
        public required string Image;
    }
}
