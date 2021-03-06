using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Animo.Web.Core.Models.Users
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}