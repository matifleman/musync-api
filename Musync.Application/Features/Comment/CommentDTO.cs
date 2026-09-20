using Musync.Application.DTOs;

namespace Musync.Application.Features.Comment
{
    public sealed record CommentDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public required UserDTO Author { get; set; }
        public required string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
