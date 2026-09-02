using MediatR;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.RemoveBandMember
{
    public sealed record RemoveBandMemberCommand(int BandId, int UserId) : IRequest<BandDTO>;
}
