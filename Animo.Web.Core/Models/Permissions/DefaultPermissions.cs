using System.Collections.Generic;

namespace Animo.Web.Core.Models.Permissions
{
    public class DefaultPermissions
    {
        // Authorize attributes
        public const string PermissionNameForAdminAccess = nameof(AdminAccess);
        public const string PermissionNameForMemberAccess = nameof(MemberAccess);

        public static readonly Permission AdminAccess = new()
        {
            Id = 1,
            DisplayName = "Admin access",
            Name = PermissionNameForAdminAccess,
        };

        public static readonly Permission MemberAccess = new()
        {
            Id = 2,
            DisplayName = "Member access",
            Name = PermissionNameForMemberAccess,
        };

        public static List<Permission> All()
        {
            return new List<Permission>
            {
                AdminAccess,
                MemberAccess,
            };
        }
    }
}