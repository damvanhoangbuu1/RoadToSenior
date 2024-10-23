using _1.RoadToSenior.Api.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace _1.RoadToSenior.Api.Filter
{
    [AttributeUsage(AttributeTargets.Class)]
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
                context.Result = new JsonResult(new { message = "Xin lỗi vì sự cố, bạn chưa đăng nhập." }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var userRoles = context.HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            if (!userRoles.Contains(_role.ToString()))
            {
                context.Result = new JsonResult(new { message = "Xin lỗi vì sự cố, bạn không có quyền truy cập." }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
