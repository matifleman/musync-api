using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Post : BaseEntity
    {
        /// <summary>
        /// Upper bound for <see cref="Caption"/>, enforced by the Application-layer validators.
        /// Lives here so both Musync.Application and Musync.Persistance can reference one number.
        /// </summary>
        public const int CaptionMaxLength = 2200;

        public int AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public string? Caption { get; set; }
        public required string Image { get; set; }
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}
