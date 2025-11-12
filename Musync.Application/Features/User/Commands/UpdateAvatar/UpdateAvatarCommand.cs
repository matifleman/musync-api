using MediatR;
using Microsoft.AspNetCore.Http;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Commands.UpdateAvatar
{
    public sealed record UpdateAvatarCommand(IFormFile newAvatar) : IRequest<UserDTO>;
}
