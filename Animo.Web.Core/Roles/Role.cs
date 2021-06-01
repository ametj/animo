﻿using Animo.Web.Core.Users;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Animo.Web.Core.Roles
{
    public class Role : IdentityRole<int>
    {
        public bool IsSystemDefault { get; set; } = false;

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}