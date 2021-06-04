using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Animo.Web.Core.Models.Users
{
    public class DefaultUsers
    {
        private static readonly PasswordHasher<User> _hasher = new();

        public static readonly User Admin = Create(1, nameof(Admin));
        public static readonly User Member = Create(2, nameof(Member));

        public static User Create(int id, string name)
        {
            var email = $"{name}@mail.com";

            return new()
            {
                Id = id,
                UserName = name,
                Email = email,
                EmailConfirmed = true,
                NormalizedUserName = name.ToUpper(),
                NormalizedEmail = email.ToUpper(),
                AccessFailedCount = 5,
                PasswordHash = _hasher.HashPassword(null, name),
                SecurityStamp = new Guid().ToString()
            };
        }

        public static List<User> All()
        {
            return new List<User>
            {
                Admin,
                Member,
            };
        }
    }
}