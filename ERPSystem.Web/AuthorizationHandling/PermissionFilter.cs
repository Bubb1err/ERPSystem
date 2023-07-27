using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ERPSystem.Web.AuthorizationHandling
{
    public class PermissionFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authService;
        private readonly PermissionRequirement _requirement;

        public PermissionFilter(IAuthorizationService authService, PermissionRequirement requirement)
        {
            _authService = authService;
            _requirement = requirement;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            AuthorizationResult result = await _authService.AuthorizeAsync(
                context.HttpContext.User, null, _requirement);

            if (!result.Succeeded) context.Result = new ChallengeResult();
        }
    }
}
