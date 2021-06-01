using Animo.Web.Core;
using Animo.Web.Core.Authentification;
using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Models.Roles;
using Animo.Web.Core.Models.Users;
using Animo.Web.Core.Services;
using Animo.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Animo.Web.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AnimoDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                foreach (var permission in DefaultPermissions.All())
                {
                    options.AddPolicy(permission.Name,
                        policy => policy.Requirements.Add(new PermissionRequirement(permission)));
                }
            });
        }

        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IBaseDbContext, AnimoDbContext>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}