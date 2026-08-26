using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.User.Commands.UpdateInstruments
{
    public sealed record UpdateInstrumentsCommand(List<int> InstrumentIds) : IRequest<UserDTO>;
}
