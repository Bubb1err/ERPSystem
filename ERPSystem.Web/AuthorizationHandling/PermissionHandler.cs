using ERPSystem.Resources;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ERPSystem.Web.AuthorizationHandling
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var claims = context.User.Claims;
            var permissionClaims = claims.Where(c => c.Type == ClaimTypes.AuthorizationDecision);

            List<Permission> permissions = new List<Permission>();

            foreach (var claim in permissionClaims)
            {
                var permission = Enum.Parse<Permission>(claim.Value);
                permissions.Add(permission);
            }

            if (requirement.Permissions.All(r => permissions.Contains(r)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}