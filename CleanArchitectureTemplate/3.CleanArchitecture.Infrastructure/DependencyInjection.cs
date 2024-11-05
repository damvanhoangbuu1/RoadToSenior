using _1.CleanArchitecture.Domain.Common;
using _2.CleanArchitecture.Application.Common.Interfaces;
using _2.CleanArchitecture.Application.Features.IRepositories;
using _2.CleanArchitecture.Application.Features.Services;
using _3.CleanArchitecture.Infrastructure.Persistence;
using _3.CleanArchitecture.Infrastructure.Repositories;
using _3.CleanArchitecture.Infrastructure.Services;
using _3.CleanArchitecture.Infrastructure.Services.Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace _3.CleanArchitecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            services.Configure<Jwt>(configuration.GetSection("Jwt"));

            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAuthService, AuthenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            return services;
        }
    }
}