using Authentication.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Authentication.API.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            var (statusCode, message) = GetExceptionDetails(exception);
            logger.LogError(exception, exception.Message);
            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(new { message }, cancellationToken);

            return true;
        }

        private (HttpStatusCode statusCode, string message) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                UserAlreadyExistsException ex => (HttpStatusCode.Conflict, ex.Message),
                RegistrationFailedException ex => (HttpStatusCode.BadRequest, ex.Message),
                LoginFailedException ex => (HttpStatusCode.Unauthorized, ex.Message),
                InvalidRefreshTokenException ex => (HttpStatusCode.Unauthorized, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "An error occurred while processing your request")
            };
        }
    }
}
