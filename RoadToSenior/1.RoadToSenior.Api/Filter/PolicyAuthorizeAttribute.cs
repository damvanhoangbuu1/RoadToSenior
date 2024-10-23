﻿using _1.RoadToSenior.Api.Models.Auth;
using _1.RoadToSenior.Api.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace _1.RoadToSenior.Api.Filter
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PolicyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly PolicyProperty _policy;
        public PolicyAuthorizeAttribute(Policy policy)
        {
            _policy = GetPolicyProperty(policy);
        }

        private static PolicyProperty GetPolicyProperty(Policy policy)
        {
            return new PolicyProperty
            {
                Role = policy == Policy.AdminEdit || policy == Policy.AdminView ? Role.Admin : Role.User,
                Permission = policy == Policy.UserView || policy == Policy.AdminView ? Permission.Wiew : Permission.Edit
            };
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

            var userPermissions = context.HttpContext.User.Claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value);

            if (!userRoles.Contains(_policy.Role.ToString()) || !userPermissions.Contains(_policy.Permission.ToString()))
            {
                context.Result = new JsonResult(new { message = "Xin lỗi vì sự cố, bạn không có quyền truy cập." }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
