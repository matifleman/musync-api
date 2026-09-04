using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Commands.UpdateProfile
{
    public sealed record UpdateProfileCommand(string FirstName, string LastName, string UserName) : IRequest<CurrentUserDTO>;
}
