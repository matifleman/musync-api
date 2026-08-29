using Microsoft.AspNetCore.Hosting;
using Musync.Api.Contracts.Exceptions;
using Musync.Api.Models;
using System.Net;

namespace Musync.Api.ExceptionHandlers
{
    public class DefaultExceptionHandler : IExceptionHandler
    {
        private readonly IWebHostEnvironment _env;

        public DefaultExceptionHandler(IWebHostEnvironment env)
        {
            _env = env;
        }

        public int Priority => int.MaxValue;

        public bool CanHandle(Exception ex) => true;

        public CustomProblemDetails Handle(Exception ex, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.InternalServerError;
            bool isDevelopment = _env.IsDevelopment();

            return new CustomProblemDetails
            {
                Title = isDevelopment ? ex.Message : "An unexpected error occurred.",
                Status = (int)statusCode,
                Type = nameof(HttpStatusCode.InternalServerError),
                Detail = isDevelopment ? ex.StackTrace : null,
            };
        }
    }
}
