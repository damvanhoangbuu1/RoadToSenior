using _3.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _4.WebAPI.Middlewares
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
                context.Items["User"] = dbContext.Users.Include(p => p.UserRoles).FirstOrDefault();
            }
            await _next(context);
        }
    }
}
