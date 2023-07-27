using ERPSystem.Resources;
using Microsoft.AspNetCore.Authorization;

namespace ERPSystem.Web.AuthorizationHandling
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission[]  Permissions { get; set; }
        public PermissionRequirement(Permission[] permissions)
        {
            Permissions = permissions;
        }
    }
}
