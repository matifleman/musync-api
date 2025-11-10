using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.Post
{
    public sealed record PostDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public required UserDTO Author { get; set; }
        public string Caption = string.Empty;
        public required string Image;
    }
}
