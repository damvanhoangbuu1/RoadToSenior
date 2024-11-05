using _3.CleanArchitecture.Infrastructure.Persistence;
using System.Diagnostics;
using System.Security.Claims;

namespace _4.CleanArchitecture.Api.Middleware
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _timer;
        private readonly ILogger<PerformanceMiddleware> _logger;

        public PerformanceMiddleware(RequestDelegate next,
                                     ILogger<PerformanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            _timer.Start();
            await _next(context);
            _timer.Stop();
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;
            if (elapsedMilliseconds > 500)
            {
                var requestName = $"{context.Request.Method}: {context.Request.Path}";
                var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
                var userName = string.Empty;

                if (!string.IsNullOrEmpty(userId))
                {
                    userName = dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId)?.Username;
                }

                _logger.LogWarning("CleanArchitecture Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName}",
                    requestName, elapsedMilliseconds, userId, userName);
            }
        }
    }
}