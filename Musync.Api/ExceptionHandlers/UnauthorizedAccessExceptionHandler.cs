using Musync.Api.Contracts.Exceptions;
using Musync.Api.Models;
using System.Net;

namespace Musync.Api.ExceptionHandlers
{
    public sealed class UnauthorizedAccessExceptionHandler : IExceptionHandler
    {
        public int Priority => 1;

        public bool CanHandle(Exception ex) => ex is UnauthorizedAccessException;

        public CustomProblemDetails Handle(Exception ex, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.Unauthorized;
            return new CustomProblemDetails
            {
                Title = ex.Message,
                Status = (int)statusCode,
                Type = nameof(UnauthorizedAccessException),
            };
        }
    }
}
