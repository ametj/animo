using Animo.Web.Core.Models.Roles;
using System.Collections.Generic;

namespace Animo.Web.Core.Models.Permissions
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}