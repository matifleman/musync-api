using AutoMapper;
using MediatR;
using Musync.Application.Contracts.Persistance;
using Musync.Application.Exceptions;

namespace Musync.Application.Features.Comment.Queries.GetPostComments
{
    public sealed class GetPostCommentsQueryHandler : IRequestHandler<GetPostCommentsQuery, List<CommentDTO>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public GetPostCommentsQueryHandler(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        // No validator: the inputs are the route id and the paging values the controller has
        // already clamped, and "the post doesn't exist" is a 404 here - same call as
        // GetPostQueryHandler.
        public async Task<List<CommentDTO>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            _ = await _postRepository.GetByIdAsync(request.PostId)
                ?? throw new NotFoundException($"Post with ID {request.PostId} not found.");

            List<Domain.Comment> comments = await _commentRepository
                .GetCommentsForPostAsync(request.PostId, request.PageNumber, request.PageSize);

            return _mapper.Map<List<CommentDTO>>(comments);
        }
    }
}
