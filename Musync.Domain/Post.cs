using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Post : BaseEntity
    {
        public int AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public string? Caption { get; set; }
        public required string Image { get; set; }
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}
