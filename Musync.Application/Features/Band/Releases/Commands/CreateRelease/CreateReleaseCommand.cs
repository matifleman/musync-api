using MediatR;
using Microsoft.AspNetCore.Http;
using Musync.Application.DTOs;
using Musync.Domain;

namespace Musync.Application.Features.Band.Releases.Commands.CreateRelease
{
    public sealed record CreateReleaseCommand(int BandId, string Title, ReleaseType Type, List<string> Songs, IFormFile Cover) : IRequest<ReleaseDetailDTO>;
}
