using Animo.Web.Api.Extensions;
using Animo.Web.Api.Services;
using Animo.Web.Api.Validators;
using Animo.Web.Core;
using Animo.Web.Core.ActionFilters;
using Animo.Web.Core.Services;
using Animo.Web.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Animo.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                                   Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AnimoDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.ConfigureAuthentication();
            services.ConfigureCors(Configuration);
            services.ConfigureJwtTokenAuthentication(Configuration);
            services.ConfigureDependencyInjection();

            services.AddAutoMapper(typeof(Startup), typeof(IBaseDbContext), typeof(AnimoDbContext));
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginValidator>());

            services.AddControllers(options =>
            {
                options.Filters.AddService<UnitOfWorkActionFilter>();
                options.Filters.AddService<BadRequestLogging>();
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Animo.Web.Api", Version = "v1" });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHostedService<DbMigratorHostedService>();
            services.AddHostedService<TMDBClientFactoryInitializationHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Animo.Web.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseHsts();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}