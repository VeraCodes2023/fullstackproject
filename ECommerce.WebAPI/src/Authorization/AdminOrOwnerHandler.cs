using Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Ecommerce.WebAPI.src.Authorization
{
    public class AdminOrOwnerHandler :
    AuthorizationHandler<AdminOrOwnerRequirement, Purchase>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrOwnerRequirement requirement, Purchase orderResource)
        {
            var user = context.User;
            var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
            var userID = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            if (userRole == Role.Admin.ToString())
            {
                context.Succeed(requirement);
            }

            if (userID == orderResource.UserId.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }

        // protected override  HandleRequirement(AuthorizationHandlerContext context, AdminOrOwnerRequirement requirement, Purchase resource)
        // {
        //     throw new NotImplementedException();
        // }
    }

    public class AdminOrOwnerRequirement : IAuthorizationRequirement { }
}
