using Animo.Web.Core.Models.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Animo.Web.Core.Authentification
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; }
    }
}