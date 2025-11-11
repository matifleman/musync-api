using MediatR;
using Musync.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musync.Application.Features.Follow.Commands.FollowUser
{
    public record FollowUserCommand(int UserIdToFollow) : IRequest<FollowResultDTO>;
}
