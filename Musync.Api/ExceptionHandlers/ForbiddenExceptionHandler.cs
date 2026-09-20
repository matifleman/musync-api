using Musync.Api.Contracts.Exceptions;
using Musync.Api.Models;
using Musync.Application.Exceptions;
using System.Net;

namespace Musync.Api.ExceptionHandlers
{
    public sealed class ForbiddenExceptionHandler : IExceptionHandler
    {
        public int Priority => 1;

        public bool CanHandle(Exception ex) => ex is ForbiddenException;

        public CustomProblemDetails Handle(Exception ex, out HttpStatusCode statusCode)
        {
            ForbiddenException forbidden = (ForbiddenException)ex;
            statusCode = HttpStatusCode.Forbidden;
            return new CustomProblemDetails
            {
                Title = forbidden.Message,
                Status = (int)statusCode,
                Type = nameof(ForbiddenException),
                Detail = forbidden.InnerException?.Message,
            };
        }
    }
}
