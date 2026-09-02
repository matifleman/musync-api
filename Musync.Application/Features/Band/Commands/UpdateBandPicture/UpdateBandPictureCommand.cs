using MediatR;
using Microsoft.AspNetCore.Http;
using Musync.Application.DTOs;

namespace Musync.Application.Features.Band.Commands.UpdateBandPicture
{
    public sealed record UpdateBandPictureCommand(int BandId, IFormFile Picture) : IRequest<BandDTO>;
}