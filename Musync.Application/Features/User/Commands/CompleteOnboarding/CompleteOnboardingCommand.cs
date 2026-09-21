using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Commands.CompleteOnboarding
{
    // No body: finishing and skipping the flow are the same thing as far as the server is
    // concerned - the flow is over and shouldn't be shown again.
    public sealed record CompleteOnboardingCommand : IRequest<CurrentUserDTO>;
}
