using System.Net;

namespace RoadToSenior.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                context.Response.Headers.Add("TimeAccess", new[] { DateTime.Now.ToString() });
                await _next(context);
            }
            catch (Exception err)
            {
                _logger.LogError($"Đã xảy ra lỗi: {err.Message}");
                await HandleExceptionAsync(context, err);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = new
            {
                context.Response.StatusCode,
                Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu.",
                Detailed = exception.Message
            };

            return context.Response.WriteAsJsonAsync(result);
        }
    }
}
