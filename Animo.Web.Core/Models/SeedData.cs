using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Models.Roles;
using Animo.Web.Core.Models.Users;
using System.Linq;

namespace Animo.Web.Core.Models
{
    public class SeedData
    {
        public static User[] BuildApplicationUsers()
        {
            return DefaultUsers.All().ToArray();
        }

        public static Role[] BuildApplicationRoles()
        {
            return DefaultRoles.All().ToArray();
        }

        public static UserRole[] BuildApplicationUserRoles()
        {
            return new[]
            {
                new UserRole
                {
                    RoleId = DefaultRoles.Admin.Id,
                    UserId = DefaultUsers.Admin.Id
                },
                new UserRole
                {
                    RoleId = DefaultRoles.Member.Id,
                    UserId = DefaultUsers.Member.Id
                }
            };
        }

        public static Permission[] BuildPermissions()
        {
            return DefaultPermissions.All().ToArray();
        }

        public static RolePermission[] BuildRolePermissions()
        {
            // Grant all permissions to admin role
            var rolePermissions = DefaultPermissions.All().Select(p =>
                new RolePermission
                {
                    PermissionId = p.Id,
                    RoleId = DefaultRoles.Admin.Id
                }).ToList();

            // Grant member access permission to member role
            rolePermissions.Add(new RolePermission
            {
                PermissionId = DefaultPermissions.MemberAccess.Id,
                RoleId = DefaultRoles.Member.Id
            });

            return rolePermissions.ToArray();
        }
    }
}