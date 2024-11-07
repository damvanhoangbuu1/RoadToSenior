using _2.Application.Interfaces;
using _2.Application.Services;
using _2.Application.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace _2.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IAuthService, AuthService>();

            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

            return services;
        }
    }
}
