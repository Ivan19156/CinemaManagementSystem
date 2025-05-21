using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    namespace YourProject.Authorization
    {
        public class ResourceOwnerOrAdminHandler : AuthorizationHandler<ResourceOwnerOrAdminRequirement>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public ResourceOwnerOrAdminHandler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                ResourceOwnerOrAdminRequirement requirement)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                var routeData = httpContext.GetRouteData();
                if (!routeData.Values.TryGetValue("id", out var idValue))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                if (!int.TryParse(idValue.ToString(), out int resourceId))
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                var userId = int.Parse(userIdClaim.Value);

                if (context.User.IsInRole("admin") || userId == resourceId)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }

                return Task.CompletedTask;
            }
        }
    }

}
