namespace _1.RoadToSenior.Api.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAuthorizationWithPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminAndEdit", policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") &&
                        context.User.HasClaim("Permission", "Edit")));

                options.AddPolicy("AdminAndView", policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") &&
                        context.User.HasClaim("Permission", "View")));
            });

            return services;
        }
    }
}
