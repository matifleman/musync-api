using AutoMapper;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;

namespace Musync.Application.Features.Post.Queries.GetFeed
{
    public sealed class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, List<PostDTO>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public GetFeedQueryHandler(
            ICurrentUserService currentUserService,
            IPostRepository postRepository,
            IPostLikeRepository postLikeRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<List<PostDTO>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
        {
            int currentUserId = _currentUserService.CurrentUserId;

            List<Domain.Post> posts = await _postRepository.GetFeedAsync(currentUserId, request.PageNumber, request.PageSize);
            HashSet<int> likedPostIds = await _postLikeRepository.GetLikedPostIdsAsync(currentUserId, posts.Select(post => post.Id));

            // One grouped query for the whole page rather than a count per row, matching how
            // the likes above are resolved. Posts with no comments are absent from the map.
            Dictionary<int, int> commentsCountByPostId = await _commentRepository.GetCommentsCountsAsync(posts.Select(post => post.Id));

            List<PostDTO> postDTOs = _mapper.Map<List<PostDTO>>(posts);
            foreach (PostDTO postDTO in postDTOs)
            {
                postDTO.Liked = likedPostIds.Contains(postDTO.Id);
                postDTO.CommentsCount = commentsCountByPostId.GetValueOrDefault(postDTO.Id);
            }

            return postDTOs;
        }
    }
}
