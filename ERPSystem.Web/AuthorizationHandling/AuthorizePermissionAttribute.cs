using ERPSystem.Resources;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.Web.AuthorizationHandling
{
    public class AuthorizePermissionAttribute : TypeFilterAttribute
    {
        public AuthorizePermissionAttribute(params Permission[] permissions)
          : base(typeof(PermissionFilter))
        {
            Arguments = new[] { new PermissionRequirement(permissions) };
            Order = Int32.MaxValue;
        }
    }
}
