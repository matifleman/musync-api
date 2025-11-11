using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Follow.Commands.UnfollowUser
{
    public record UnfollowUserCommand(int UserIdToUnfollow) : IRequest<FollowResultDTO>;
}

