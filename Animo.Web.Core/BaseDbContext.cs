using Animo.Web.Core.Permissions;
using Animo.Web.Core.Roles;
using Animo.Web.Core.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Animo.Web.Core
{
    public class BaseDbContext<T> : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken> where T : DbContext
    {
        public BaseDbContext(DbContextOptions<T> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Permission>()
                .ToTable("Permission")
                .HasData(SeedData.BuildPermissions());

            builder.Entity<Role>()
                .ToTable("Role")
                .HasData(SeedData.BuildApplicationRoles());

            builder.Entity<User>()
                .ToTable("User")
                .HasData(SeedData.BuildApplicationUsers());

            builder.Entity<RolePermission>(b =>
            {
                b.ToTable("RolePermission");

                b.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                b.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(pt => pt.RoleId);

                b.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId);

                b.HasData(SeedData.BuildRolePermissions());
            });

            builder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRole");

                b.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                b.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                b.HasData(SeedData.BuildApplicationUserRoles());
            });

            builder.Entity<UserClaim>().ToTable("UserClaim");
            builder.Entity<UserLogin>().ToTable("UserLogin");
            builder.Entity<RoleClaim>().ToTable("RoleClaim");
            builder.Entity<UserToken>().ToTable("UserToken");
        }
    }
}