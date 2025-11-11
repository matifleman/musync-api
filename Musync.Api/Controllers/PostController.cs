using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Application.Features.Post;
using Musync.Application.Features.Post.Commands;
using Musync.Application.Features.Post.Queries.GetUserPosts;

namespace Musync.Api.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<PostDTO>> CreatePost([FromForm] CreatePostCommand command)
        {
            PostDTO createdPost = await _mediator.Send(command);
            return Created($"/api/posts/{createdPost.Id}", createdPost);
        }

        [Authorize]
        [HttpGet("author/{authorId}")]
        [ProducesResponseType(typeof(List<PostDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<PostDTO>>> GetPostsByAuthor([FromRoute] int authorId)
        {
            List<PostDTO> posts = await _mediator.Send(new GetUserPostsQuery(authorId));
            return Ok(posts);
        }

    }
}
