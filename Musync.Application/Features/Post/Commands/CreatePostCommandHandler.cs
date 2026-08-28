using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Musync.Application.Common;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Post.Commands
{
    public sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDTO>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ICurrentUserService _currentUserService;

        public CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper, IWebHostEnvironment env, ICurrentUserService currentUserService)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _env = env;
            _currentUserService = currentUserService;
        }
        public async Task<PostDTO> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            CreatePostCommandValidator validator = new CreatePostCommandValidator();
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid post", validationResult);

            string imagePath = await SaveImage(request.Image, cancellationToken);

            Domain.Post postToCreate = new Domain.Post
            {
                AuthorId = _currentUserService.CurrentUserId,
                Caption = request.Caption,
                Image = imagePath
            };

            Domain.Post createdPost = await _postRepository.CreateAsync(postToCreate);

            return _mapper.Map<PostDTO>(createdPost);
        }

        private async Task<string> SaveImage(IFormFile image, CancellationToken cancellationToken)
        {
            string imagePath = string.Empty;

            string fileName = ImageUploadValidator.GenerateSafeFileName(image.FileName);
            string imagesDirectory = Path.Combine(_env.WebRootPath, "images");
            string savePath = Path.Combine(imagesDirectory, fileName);

            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            using var stream = new FileStream(savePath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            imagePath = $"/images/{fileName}";

            return imagePath;
        }
    }
}
