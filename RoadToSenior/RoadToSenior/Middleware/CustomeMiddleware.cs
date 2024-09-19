using Microsoft.AspNetCore.Http;

namespace RoadToSenior.Middleware
{
    public class CustomeMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("TimeAccess", new[] { DateTime.Now.ToString() });
            await _next(context);
        }
    }
}
