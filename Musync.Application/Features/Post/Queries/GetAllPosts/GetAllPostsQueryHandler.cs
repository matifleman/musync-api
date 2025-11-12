using AutoMapper;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;

namespace Musync.Application.Features.Post.Queries.GetAllPosts
{
    public sealed class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, List<PostDTO>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IMapper _mapper;

        public GetAllPostsQueryHandler(
            ICurrentUserService currentUserService, 
            IPostRepository postRepository, 
            IPostLikeRepository postLikeRepository, 
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _mapper = mapper;
        }
        public async Task<List<PostDTO>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<Domain.Post> posts =  await _postRepository.GetAllAsync();
            List<PostDTO> postDTOs = _mapper.Map<List<PostDTO>>(posts);
            int currentUserId = _currentUserService.CurrentUserId;
            IReadOnlyList<Domain.PostLike> likedPosts = await _postLikeRepository.GetLikesByUserIdAsync(currentUserId);

            foreach (var postDTO in postDTOs)
            {
                if (likedPosts.Any(like => like.PostId == postDTO.Id)) postDTO.Liked = true;
            }

            return postDTOs;
        }
    }
}
