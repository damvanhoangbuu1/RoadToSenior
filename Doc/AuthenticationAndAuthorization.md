### [Home](../README.md)

# 1. Lý thuyết
## 1.1. Authentication

Authentication là quá trình xác minh danh tính của người dùng hoặc thực thể, thường là thông qua tên người dùng, mật khẩu, hoặc các phương thức xác thực khác như token, OAuth, hoặc OpenID Connect.

**Các loại Authentication phổ biến:**
* **Cookie-based Authentication:** Thường được dùng cho các ứng dụng web với người dùng tương tác qua trình duyệt. Thông tin xác thực được lưu trữ trong cookie.
* **JWT Bearer Token Authentication:** Được sử dụng nhiều trong các API. Người dùng sẽ gửi JWT (JSON Web Token) để chứng minh danh tính.
* **OAuth2 và OpenID Connect:** Xác thực thông qua dịch vụ bên thứ ba như Google, Facebook, Microsoft, GitHub, v.v.

## 1.2. Authorization

Authorization là quá trình xác định người dùng có quyền truy cập tài nguyên hay thực hiện hành động cụ thể trong hệ thống hay không.

**Các loại phân quyền:**
* **Role-based Authorization:** Dựa trên vai trò của người dùng (Admin, User, Manager...).
* **Policy-based Authorization:** Phân quyền dựa trên một tập hợp các điều kiện (các policy), không chỉ dựa trên vai trò.
* **Claims-based Authorization:** Phân quyền dựa trên các thông tin của người dùng chứa trong claims (ví dụ: email, username, department).

# 2. Cách triển khai Authentication và Authorization (net6.0)

## 2.1. Cài đặt thư viện

Cài đặt bằng nuget JWTBearer hoặc run donet-cli:

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

## 2.2. Cấu hình `appsettings.json`

Thêm thông tin liên quan đến JWT trong `appsettings.json`:

```json
"JwtSettings": {
  "SecretKey": "YourSecretKeyHere",  // Khóa bí mật để mã hóa JWT(yêu cầu lớn hơn 32 ký tự)
  "Issuer": "yourdomain.com",        // Issuer của token
  "Audience": "yourdomain.com",      // Audience của token
  "ExpiryMinutes": 60                // Thời gian hết hạn của token
}
```

## 2.2. Cấu hình dịch vụ Authentication

**Bước 1: Đăng ký dịch vụ Authentication**
Trong `Program.cs`, đăng ký JWT Authentication trong container dịch vụ:

```c#
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình từ appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
// Cấu hình và đăng ký JwtSettings vào hệ thống DI (Dependency Injection)
// Lấy thông tin từ section "JwtSettings" trong file cấu hình (appsettings.json)
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

// Đăng ký dịch vụ Authentication và JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();
```

Lưu ý, tạo class JwtSettings có các property giống hết trong cấu hình file `appsettings.json`

```c#
public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
}
```

**Bước 2: Thêm Authentication và Authorization vào Middleware**

Thêm các middleware cho Authentication và Authorization vào pipeline của ứng dụng (Authentication trước Authorization):

```c#
app.UseAuthentication();  // Đảm bảo gọi trước UseAuthorization
app.UseAuthorization();
```

## 2.3 Tạo class để sinh và xác thực JWT token

```c#
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenHelper(IOptions<JwtSettings> jwtSettings)
    {
        // lấy giá trị JwtSettings đã được config vào hệ thống DI
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(string username, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

**Lưu ý:** tuỳ trường hợp bạn có thể thêm nhiều thông tin khác của user vào `Claim` theo yêu cầu của bảng thân
Ví dụ:
```c#
var claims = new[]
{
    new Claim(ClaimTypes.Name, username),
    new Claim(ClaimTypes.Role, role),
    new Claim("Permission", user.Permission.ToString()),
};
```


## 2.4 Tạo API Login để sinh JWT

```c#
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtTokenHelper _jwtTokenHelper;

    public AuthController(JwtTokenHelper jwtTokenHelper)
    {
        _jwtTokenHelper = jwtTokenHelper;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Xác thực người dùng từ database hoặc dịch vụ
        if (model.Username == "admin" && model.Password == "password")
        {
            var token = _jwtTokenHelper.GenerateToken(model.Username, "Admin");
            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

## 2.5. Áp dụng Authorization
Sử dụng `[Authorize]` để bảo vệ các api yêu cầu JWT

Ví dụ dưới đây là yêu cầu đăng nhập mới truy cập được api:
```c#
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [Authorize]
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok($"Sản phẩm có ID: {id}");
    }
}
```

Ví dụ dưới đây là yêu cầu người dùng có `Roles = "Admin"` mới truy cập được api:

```c#
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok($"Sản phẩm có ID: {id}");
    }
}
```

Nếu không đáp ứng yêu cầu Authorize sẽ trả về với status 403.

## 2.6. Sử dụng nâng cao Authorization

**Ngoài Authorize bằng Roles có thể dùng Policy (ta có thể kết hợp nhiều điều kiện bằng thông tin đã lưu trong claim)**

```c#
builder.Services.AddAuthorization(options =>
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
```

Như ví dụ trên sẽ tạo ra 2 Policy là `AdminAndEdit` và `AdminAndView` dựa vào điều kiện của Role và Permission trong claim.

Sử dụng `[Authorize(Policy = "<Policy name>")]` trong controller để Authorize bằng Policy:

```c#
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [Authorize(Policy = "AdminAndEdit")]
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        return Ok($"Sản phẩm có ID: {id}");
    }
}
```

Khi đó nếu user đăng nhập có `Roles == "Admin"` và `Permission == "Edit"` mới truy cập được api, nếu không kết quả trả về sẽ có staus 403.

## 2.7. Nâng cao
**Có thể clean code bằng cách tạo Extensions block các dòng code thêm dịch vụ Authen và Authorize dài dòn thành gọn gàng**

Tạo file `JWTAuthenticationExtension.cs` để tạo extension cho Authen:

```c#
using _1.RoadToSenior.Api.Helper;
using _1.RoadToSenior.Api.Models.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace _1.RoadToSenior.Api.Extensions
{
    public static class JWTAuthenticationExtension
    {
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            services.AddSingleton<JwtTokenHelper>();

            return services;
        }
    }
}
```

`Program.cs`

```c
builder.Services.AddJWTAuthentication(builder.Configuration);
// author service here

var app = builder.Build();
```

Tạo file `AuthorizationExtension.cs` để tạo extension cho Authorize:

```c#
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
```

`Program.cs`

```c
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddAuthorizationWithPolicy();

var app = builder.Build();
```

**Thêm swagger để test api**

**Bước 1:** Add nugget Swashbuckle
**Bước 2:** Add `SwaggerExtension.cs`
```c#
using Microsoft.OpenApi.Models;

namespace _1.RoadToSenior.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Documentation",
                    Contact = new OpenApiContact
                    {
                        Name = "damvanhoangbuu1",
                        Email = "damhoangbuu@gmail.com",
                        Url = new Uri("https://github.com/damvanhoangbuu1")
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter your token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
```
**Bước 3:**
`Program.cs`

```c#
builder.Services.AddSwaggerDocumentation();

builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddAuthorizationWithPolicy();

var app = builder.Build();
```

**Bước 4.**

Config comment xml trên swagger

Thêm vào `SwaggerExtension.cs`
```c#
public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IWebHostEnvironment environment)
{
    if (environment.IsDevelopment())
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }
    // Anthor swagger config

    return services
}
```

Thêm vào file `.csprj` 

```
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

## 2.8. Custom Filter Authorize

```c#
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role _role;
    public AuthorizeAttribute(Role role) { 
        _role = role;
    }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity == null || !context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new JsonResult(new { message = "Sorry, you are not logged in." }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        var userRoles = context.HttpContext.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);

        if (!userRoles.Contains(_role.ToString()))
        {
            context.Result = new JsonResult(new { message = "Sorry, you do not have access." }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
```

Tạo class mới kế thừa class Attribute và interface IAuthorizationFilter

Trong phương thức OnAuthorization có thể tuỳ chỉnh filter theo ý của bạn.

Khi sử dụng chỉ cần dùng theo cú pháp `[Attribute-name(role-name)]`, `role-name` tuỳ theo hàm dựng có thể là string hoặc enum, ở ví dụ trên dùng enum.

```c#
[Authorize(Role.Admin)]
[HttpGet("GetWeatherForecast")]
public IEnumerable<WeatherForecast> Get()
{
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
}
```