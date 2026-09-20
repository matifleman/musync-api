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
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public GetUserPostsQueryHandler(
            UserManager<ApplicationUser> userManager,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }
        public async Task<List<PostDTO>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            GetUserPostsQueryValidator validator = new GetUserPostsQueryValidator(_userManager);
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid request", validationResult);

            List<Domain.Post> userPosts = await _postRepository.GetPostsByAuthorIdAsync(request.authorId);

            Dictionary<int, int> commentsCountByPostId = await _commentRepository.GetCommentsCountsAsync(userPosts.Select(post => post.Id));

            List<PostDTO> postDTOs = _mapper.Map<List<PostDTO>>(userPosts);
            // Note: this handler still doesn't resolve Liked, unlike every other PostDTO
            // producer. That gap predates comments and is left alone here on purpose.
            foreach (PostDTO postDTO in postDTOs)
                postDTO.CommentsCount = commentsCountByPostId.GetValueOrDefault(postDTO.Id);

            return postDTOs;
        }
    }
}
