using Musync.Api.Contracts.Exceptions;
using Musync.Api.Models;
using Musync.Application.Exceptions;
using System.Net;

namespace Musync.Api.ExceptionHandlers
{
    public sealed class BadRequestExceptionHandler : IExceptionHandler
    {
        public int Priority => 1;
        public bool CanHandle(Exception ex) => ex is BadRequestException;

        public CustomProblemDetails Handle(Exception ex, out HttpStatusCode statusCode)
        {
            BadRequestException badRequest = (BadRequestException)ex;
            statusCode = HttpStatusCode.BadRequest;
            return new CustomProblemDetails
            {
                Title = badRequest.Message,
                Status = (int)statusCode,
                Detail = badRequest.InnerException?.Message,
                Type = nameof(BadRequestException),
                Errors = badRequest.ValidationErrors
            };
        }
    }
}
