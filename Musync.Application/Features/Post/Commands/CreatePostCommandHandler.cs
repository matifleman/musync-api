using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Post.Commands
{
    public sealed class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDTO>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _env = env;
            _userManager = userManager;
        }
        public async Task<PostDTO> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            CreatePostCommandValidator validator = new CreatePostCommandValidator(_userManager);
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid post", validationResult);

            string imagePath = await SaveImage(request.Image, cancellationToken);

            Domain.Post postToCreate = new Domain.Post
            {
                AuthorId = request.AuthorId,
                Caption = request.Caption,
                Image = imagePath
            };

            Domain.Post createdPost = await _postRepository.CreateAsync(postToCreate);

            return _mapper.Map<PostDTO>(createdPost);
        }

        private async Task<string> SaveImage(IFormFile image, CancellationToken cancellationToken)
        {
            string imagePath = string.Empty;

            string fileName = $"{Guid.NewGuid()}_{image.FileName}";
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
