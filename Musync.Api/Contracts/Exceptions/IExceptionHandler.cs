using Musync.Api.Models;
using System.Net;

namespace Musync.Api.Contracts.Exceptions
{
    public interface IExceptionHandler
    {
        int Priority { get; } // lower number == higher priority
        bool CanHandle(Exception ex);
        CustomProblemDetails Handle(Exception ex, out HttpStatusCode statusCode);   
    }
}
