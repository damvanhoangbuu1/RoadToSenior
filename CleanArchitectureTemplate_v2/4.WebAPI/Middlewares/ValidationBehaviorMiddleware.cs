using _2.Application.Common;
using FluentValidation;

namespace _4.WebAPI.Middlewares
{
    public class ValidationBehaviorMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationBehaviorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var validators = context.RequestServices.GetServices<IValidator>();

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync((IValidationContext)context.Request);

                if (!result.IsValid)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(Result.Failure(result.Errors.Select(e => e.ErrorMessage)));
                    return;
                }
            }

            await _next(context);
        }
    }
}
