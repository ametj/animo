using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Animo.Web.Core.Auth
{
    public record PermissionRequirement(Permission Permission) : IAuthorizationRequirement;

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var hasPermission = await _permissionService.IsUserGrantedToPermissionAsync(context.User.Identity.Name, requirement.Permission.Name);
            if (hasPermission)
            {
                context.Succeed(requirement);
            }
        }
    }
}