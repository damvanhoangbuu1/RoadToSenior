using _1.Domain.Entities;
using _1.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _4.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private List<string> _roles { get; set; }

        public AuthorizeAttribute(params RoleType[] role)
        {
            _roles = role.Select(p => p.ToString()).ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { message = "Sorry, you are not logged in." }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            // skip if not set roles
            if (_roles == null || _roles?.Count == 0)
            {
                return;
            }

            // Authorization
            var user = (User)context.HttpContext.Items["User"];

            foreach (var role in _roles)
            {
                if (!user.UserRoles.Any(p => p.Role.RoleType.ToString() == role))
                {
                    context.Result = new JsonResult(new { message = "Sorry, you do not have access." }) { StatusCode = StatusCodes.Status401Unauthorized };
                    break;
                }
            }
        }
    }
}