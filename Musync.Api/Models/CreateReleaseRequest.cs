using Microsoft.AspNetCore.Http;
using Musync.Domain;

namespace Musync.Api.Models
{
    public sealed record CreateReleaseRequest(string Title, ReleaseType Type, List<string> Songs, IFormFile Cover);
}
