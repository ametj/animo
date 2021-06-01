using Animo.Web.Core.Models.Roles;
using Microsoft.AspNetCore.Identity;

namespace Animo.Web.Core.Models.Users
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}