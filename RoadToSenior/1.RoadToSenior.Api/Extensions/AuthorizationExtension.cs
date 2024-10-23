using _1.RoadToSenior.Api.Models.Auth;
using _1.RoadToSenior.Api.Models.Common;

namespace _1.RoadToSenior.Api.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAuthorizationWithPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.AdminEdit.ToString(), policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole(Role.Admin.ToString()) &&
                        context.User.HasClaim("Permission", Permission.Edit.ToString())));

                options.AddPolicy(Policy.AdminView.ToString(), policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole(Role.Admin.ToString()) &&
                        context.User.HasClaim("Permission", Permission.Wiew.ToString())));
            });

            return services;
        }
    }
}
