using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.UpdateBandName
{
    public sealed record UpdateBandNameCommand(int BandId, string Name) : IRequest<BandDTO>;
}