using Musync.Api.Contracts.Exceptions;
using Musync.Api.Models;
using Newtonsoft.Json;
using System.Net;

namespace HR.LeaveManagement.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                using var scope = serviceProvider.CreateScope();
                IEnumerable<IExceptionHandler> handlers = scope.ServiceProvider.GetServices<IExceptionHandler>();
                await HandleExceptionAsync(httpContext, ex, handlers);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, IEnumerable<IExceptionHandler> handlers)
        {
            HttpStatusCode statusCode;
            CustomProblemDetails problem;

            IExceptionHandler? handler = handlers
                .Where(h => h.CanHandle(ex))
                .OrderBy(h => h.Priority)
                .First();

            problem = handler.Handle(ex, out statusCode);

            httpContext.Response.StatusCode = (int)statusCode;
            var logMessage = JsonConvert.SerializeObject(problem);
            _logger.LogError(logMessage);
            await httpContext.Response.WriteAsJsonAsync(problem);

        }
    }
}
