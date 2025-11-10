using MediatR;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.Features.Post;
using Musync.Application.Features.Post.Commands;

namespace Musync.Api.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PostDTO>> CreatePost([FromForm] CreatePostCommand command)
        {
            PostDTO createdPost = await _mediator.Send(command);
            return CreatedAtAction("/post/:id", new { id = createdPost.Id }, createdPost);
        }
    }
}
