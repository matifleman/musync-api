using MediatR;
using Microsoft.AspNetCore.Http;

namespace Musync.Application.Features.Post.Commands
{
    public sealed class CreatePostCommand : IRequest<PostDTO>
    {
        public int AuthorId { get; set; }
        public string Caption { get; set; } = string.Empty;
        public required IFormFile Image { get; set; }
    }
}
