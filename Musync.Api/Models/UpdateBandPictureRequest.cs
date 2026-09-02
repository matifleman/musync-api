using Microsoft.AspNetCore.Http;

namespace Musync.Api.Models
{
    public sealed record UpdateBandPictureRequest(IFormFile Picture);
}
