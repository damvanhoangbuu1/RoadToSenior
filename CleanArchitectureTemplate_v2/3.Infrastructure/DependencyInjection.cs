using _1.Domain.Interfaces;
using _3.Infrastructure.Persistence;
using _3.Infrastructure.Repositories;
using _3.Infrastructure.Services;
using _3.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace _3.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = new JwtSettings();
                    configuration.GetSection("JwtSettings").Bind(jwtSettings);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            return services;
        }
    }
}