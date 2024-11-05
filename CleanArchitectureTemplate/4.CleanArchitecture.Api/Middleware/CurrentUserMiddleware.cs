using _3.CleanArchitecture.Infrastructure.Persistence;
using System.Security.Claims;

namespace _4.CleanArchitecture.Api.Middleware
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId != null)
            {
                context.Items["User"] = dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            }
            await _next(context);
        }
    }
}