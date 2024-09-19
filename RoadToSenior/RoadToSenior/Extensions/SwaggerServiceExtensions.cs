using Microsoft.OpenApi.Models;

namespace RoadToSenior.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Documentation",
                    Description = "API dành cho các dịch vụ",
                    Contact = new OpenApiContact
                    {
                        Name = "Road To Senior",
                        Email = "damhoangbuu@gmail.com",
                        Url = new Uri("https://example.com")
                    }
                });

                // Cấu hình JWT Authorization cho Swagger nếu cần
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Vui lòng nhập JWT theo định dạng: Bearer {token}",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = "Bearer"
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });

            return services;
        }
    }
}
