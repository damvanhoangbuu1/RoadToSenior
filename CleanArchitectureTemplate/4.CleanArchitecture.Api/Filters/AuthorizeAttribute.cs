using _1.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _4.CleanArchitecture.Api.Filters
{
    /// <summary>
    /// Specifies the class this attribute is applied to requires authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        public AuthorizeAttribute()
        { }

        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        private string[] _roles { get; set; }

        /// <summary>
        /// Gets or sets the policy name that determines access to the resource.
        /// </summary>
        public AuthorizeAttribute(params string[] role)
        {
            _roles = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if action is decorated with [AllowAnonymous] attribute
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { message = "Sorry, you are not logged in." }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            // Authorization
            var user = (User)context.HttpContext.Items["User"];

            foreach (var role in _roles)
            {
                if (string.IsNullOrWhiteSpace(role) || !user.Roles.Any(r => r.Contains(role)))
                {
                    context.Result = new JsonResult(new { message = "Sorry, you do not have access." }) { StatusCode = StatusCodes.Status401Unauthorized };
                    break;
                }
            }
        }
    }
}