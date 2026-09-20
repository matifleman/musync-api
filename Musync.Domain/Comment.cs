using Musync.Domain.Common;

namespace Musync.Domain
{
    public sealed class Comment : BaseEntity
    {
        /// <summary>
        /// Upper bound for <see cref="Text"/>, enforced by the Application-layer validators.
        /// Lives here so both Musync.Application and Musync.Persistance can reference one number.
        /// </summary>
        public const int TextMaxLength = 500;

        public int PostId { get; set; }
        public Post? Post { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser? Author { get; set; }
        public required string Text { get; set; }
    }
}
