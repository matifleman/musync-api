using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musync.Api.Models;
using Musync.Application.Common;
using Musync.Application.Features.Comment;
using Musync.Application.Features.Comment.Queries.GetPostComments;
using Musync.Application.Features.Like.Commands.DeletePostLike;
using Musync.Application.Features.Like.Commands.LikePost;
using Musync.Application.Features.Post;
using Musync.Application.Features.Post.Commands;
using Musync.Application.Features.Post.Commands.DeletePost;
using Musync.Application.Features.Post.Commands.UpdatePostCaption;
using Musync.Application.Features.Post.Queries.GetFeed;
using Musync.Application.Features.Post.Queries.GetPost;
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

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(ImageUploadValidator.MaxFileSizeBytes + 1024 * 1024)]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [Authorize]
        [HttpGet("feed")]
        [ProducesResponseType(typeof(List<PostDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<PostDTO>>> GetFeed(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<PostDTO> posts = await _mediator.Send(new GetFeedQuery(pageNumber, pageSize));
            return Ok(posts);
        }

        [Authorize]
        [HttpGet("{postId}")]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostDTO>> GetPost([FromRoute] int postId)
        {
            PostDTO post = await _mediator.Send(new GetPostQuery(postId));
            return Ok(post);
        }

        [Authorize]
        [HttpPatch("{postId}")]
        [ProducesResponseType(typeof(PostDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostDTO>> UpdatePostCaption([FromRoute] int postId, [FromBody] UpdatePostCaptionRequest request)
        {
            PostDTO post = await _mediator.Send(new UpdatePostCaptionCommand(postId, request.Caption));
            return Ok(post);
        }

        [Authorize]
        [HttpDelete("{postId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePost([FromRoute] int postId)
        {
            await _mediator.Send(new DeletePostCommand(postId));
            return NoContent();
        }

        [Authorize]
        [HttpPost("{postId}/like")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> LikePost([FromRoute] int postId)
        {
            await _mediator.Send(new LikePostCommand(postId));
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{postId}/like")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RemovePostLike([FromRoute] int postId)
        {
            await _mediator.Send(new DeletePostLikeCommand(postId));
            return NoContent();
        }

        [Authorize]
        [HttpGet("{postId}/comments")]
        [ProducesResponseType(typeof(List<CommentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<CommentDTO>>> GetPostComments(
            [FromRoute] int postId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (pageSize > 50) pageSize = 50;
            if (pageSize < 1) pageSize = 20;
            if (pageNumber < 1) pageNumber = 1;

            List<CommentDTO> comments = await _mediator.Send(new GetPostCommentsQuery(postId, pageNumber, pageSize));
            return Ok(comments);
        }
    }
}
