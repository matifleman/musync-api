using AutoMapper;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Contracts.Services;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Post.Queries.GetPost
{
    public sealed class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDTO>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IMapper _mapper;

        public GetPostQueryHandler(
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

        // No validator: the only input is the route id, and "the post doesn't exist" is a 404
        // here rather than the 400 the Like/Unlike validators produce for the same condition.
        public async Task<PostDTO> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            int currentUserId = _currentUserService.CurrentUserId;

            Domain.Post post = await _postRepository.GetPostWithAuthorAsync(request.PostId)
                ?? throw new NotFoundException($"Post with ID {request.PostId} not found.");

            PostDTO postDTO = _mapper.Map<PostDTO>(post);
            postDTO.Liked = await _postLikeRepository.HasUserLikedPost(currentUserId, post.Id);

            return postDTO;
        }
    }
}
