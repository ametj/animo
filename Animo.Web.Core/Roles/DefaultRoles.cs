using System.Collections.Generic;

namespace Animo.Web.Core.Roles
{
    public static class DefaultRoles
    {
        public static readonly Role Admin = Create(1, nameof(Admin));
        public static readonly Role Member = Create(2, nameof(Member));

        public static Role Create(int id, string name)
        {
            return new()
            {
                Id = id,
                Name = name,
                NormalizedName = name.ToUpper(),
                IsSystemDefault = true
            };
        }

        public static List<Role> All()
        {
            return new List<Role>
            {
                Admin,
                Member
            };
        }
    }
}