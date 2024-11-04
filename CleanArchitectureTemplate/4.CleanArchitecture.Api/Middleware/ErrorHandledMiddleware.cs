using System.Net;

namespace _4.CleanArchitecture.Api.Middleware
{
    public class ErrorHandledMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandledMiddleware> _logger;

        public ErrorHandledMiddleware(RequestDelegate next,
                                           ILogger<ErrorHandledMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var requestName = $"{context.Request.Method}: {context.Request.Path}";

                _logger.LogError(ex, "CleanArchitecture Request: Unhandled Exception for Request {Name}", requestName);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("An error occurred while processing the request.");
            }
        }
    }
}
