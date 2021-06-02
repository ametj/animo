using Animo.Web.Core;
using Animo.Web.Core.ActionFilters;
using Animo.Web.Core.Auth;
using Animo.Web.Core.Models.Permissions;
using Animo.Web.Core.Models.Roles;
using Animo.Web.Core.Models.Users;
using Animo.Web.Core.Services;
using Animo.Web.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Animo.Web.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static SymmetricSecurityKey _signingKey;
        private static JwtTokenConfiguration _jwtTokenConfiguration;

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

        public static void ConfigureJwtTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            _signingKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"]));

            _jwtTokenConfiguration = new JwtTokenConfiguration
            {
                Issuer = configuration["Authentication:JwtBearer:Issuer"],
                Audience = configuration["Authentication:JwtBearer:Audience"],
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(60),
            };

            services.Configure<JwtTokenConfiguration>(config =>
            {
                config.Audience = _jwtTokenConfiguration.Audience;
                config.EndDate = _jwtTokenConfiguration.EndDate;
                config.Issuer = _jwtTokenConfiguration.Issuer;
                config.StartDate = _jwtTokenConfiguration.StartDate;
                config.SigningCredentials = _jwtTokenConfiguration.SigningCredentials;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtTokenConfiguration.Issuer,
                    ValidAudience = _jwtTokenConfiguration.Audience,
                    IssuerSigningKey = _signingKey
                };
            });
        }

        public static void ConfigureSmtp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(serviceProvider => new SmtpClient
            {
                Host = configuration["Email:Smtp:Host"],
                Port = int.Parse(configuration["Email:Smtp:Port"]),
                Credentials = new NetworkCredential(configuration["Email:Smtp:Username"], configuration["Email:Smtp:Password"]),
                EnableSsl = bool.Parse(configuration["Email:Smtp:EnableSsl"])
            });
        }

        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IBaseDbContext, AnimoDbContext>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UnitOfWorkActionFilter>();
            services.AddScoped<BadRequestLogging>();
        }
    }
}