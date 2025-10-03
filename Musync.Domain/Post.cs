using Musync.Domain.Common;
using Musync.Identity.Models;

namespace Musync.Domain
{
    public sealed class Post : BaseEntity
    {
        public int AuthorId { get; set; }
        public required ApplicationUser Author { get; set; }
        public string? Caption;
        public required string Image;
    }
}
