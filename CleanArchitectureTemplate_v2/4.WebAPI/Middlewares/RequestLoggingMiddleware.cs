using _3.Infrastructure.Persistence;
using System.Security.Claims;

namespace _4.WebAPI.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next,
                                 ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            var requestName = $"{context.Request.Method}: {context.Request.Path}";
            var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            string? userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId)?.Username;
            }

            _logger.LogInformation("CleanArchitecture Request: {Name} {@UserId} {@UserName}",
                requestName, userId, userName);
            await _next(context);
        }
    }
}
