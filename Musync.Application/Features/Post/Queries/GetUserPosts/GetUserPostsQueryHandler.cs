using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Exceptions;
using Musync.Domain;

namespace Musync.Application.Features.Post.Queries.GetUserPosts
{
    public sealed class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, List<PostDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetUserPostsQueryHandler(UserManager<ApplicationUser> userManager, IPostRepository postRepository, IMapper mapper)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<List<PostDTO>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            GetUserPostsQueryValidator validator = new GetUserPostsQueryValidator(_userManager);
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid request", validationResult);

            List<Domain.Post> userPosts = await _postRepository.GetPostsByAuthorIdAsync(request.authorId);

            return _mapper.Map<List<PostDTO>>(userPosts);
        }
    }
}
