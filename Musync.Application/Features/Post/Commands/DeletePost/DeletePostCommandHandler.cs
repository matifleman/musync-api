using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Post.Commands.DeletePost
{
    public sealed class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostRepository _postRepository;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DeletePostCommandHandler> _logger;

        public DeletePostCommandHandler(
            ICurrentUserService currentUserService,
            IPostRepository postRepository,
            IWebHostEnvironment env,
            ILogger<DeletePostCommandHandler> logger)
        {
            _currentUserService = currentUserService;
            _postRepository = postRepository;
            _env = env;
            _logger = logger;
        }

        // No validator: the only input is the route id, and existence/ownership are 404/403
        // concerns the handler checks itself.
        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            Domain.Post post = await _postRepository.GetByIdAsync(request.PostId)
                ?? throw new NotFoundException($"Post with ID {request.PostId} not found.");

            // 404 before 403, so a wrong id never comes back as "you're not allowed".
            if (post.AuthorId != _currentUserService.CurrentUserId)
                throw new ForbiddenException("Only the author can delete this post.");

            string imagePath = post.Image;

            // Row first: the post's likes go with it through the PostLikes cascade, and if this
            // fails nothing has been lost. Deleting the file first would risk a surviving post
            // that renders a broken image.
            await _postRepository.DeleteAsync(post);
            TryDeleteImageFile(imagePath);

            return Unit.Value;
        }

        private void TryDeleteImageFile(string imagePath)
        {
            string imagesRoot = Path.GetFullPath(Path.Combine(_env.WebRootPath, "images"));

            // Post.Image is read back out of the database, so it is treated as untrusted here:
            // TrimStart keeps Path.Combine from honouring it as an absolute path, and the
            // containment check below catches anything that still escapes wwwroot/images.
            string fullPath = Path.GetFullPath(Path.Combine(_env.WebRootPath, imagePath.TrimStart('/', '\\')));

            if (!fullPath.StartsWith(imagesRoot + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            {
                _logger.LogWarning("Refusing to delete image outside wwwroot/images: {ImagePath}", imagePath);
                return;
            }

            // Broad catch on purpose. The post row is already gone, so no failure here should
            // fail the request - and File.Delete's UnauthorizedAccessException in particular
            // would be mapped to 401 by UnauthorizedAccessExceptionHandler, telling the caller
            // to log in again after a delete that actually succeeded.
            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not delete image file {FullPath}", fullPath);
            }
        }
    }
}
