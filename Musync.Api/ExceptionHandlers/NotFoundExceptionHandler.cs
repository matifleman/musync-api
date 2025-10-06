using Musync.Api.Contracts.Exceptions;
using Musync.Api.Models;
using Musync.Application.Exceptions;
using System.Net;

namespace Musync.Api.ExceptionHandlers
{
    public sealed class NotFoundExceptionHandler : IExceptionHandler
    {
        public int Priority => 1;

        public bool CanHandle(Exception ex) => ex is NotFoundException;

        public CustomProblemDetails Handle(Exception ex, out HttpStatusCode statusCode)
        {
            NotFoundException notFound = (NotFoundException)ex;
            statusCode = HttpStatusCode.NotFound;
            return new CustomProblemDetails
            {
                Title = notFound.Message,
                Status = (int)statusCode,
                Type = nameof(NotFoundException),
                Detail = notFound.InnerException?.Message,
            };
        }
    }
}
