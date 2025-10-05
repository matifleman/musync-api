using Musync.Api.Contracts.Exceptions;
using Musync.Api.Models;
using System.Net;

namespace Musync.Api.ExceptionHandlers
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        public int Priority => int.MaxValue;

        public bool CanHandle(Exception ex) => true;

        public CustomProblemDetails Handle(Exception ex, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.InternalServerError;
            return new CustomProblemDetails
            {
                Title = ex.Message,
                Status = (int)statusCode,
                Type = nameof(HttpStatusCode.InternalServerError),
                Detail = ex.StackTrace,
            };
        }
    }
}
