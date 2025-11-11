using Musync.Application.DTOs;

namespace Musync.Application.Features.Post
{
    public sealed record PostDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public required UserDTO Author { get; set; }
        public string Caption { get; set; } = string.Empty;
        public required string Image { get; set; }
        public bool Liked { get; set; } = false;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
