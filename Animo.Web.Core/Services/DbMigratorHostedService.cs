using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Animo.Web.Core.Services
{
    public class DbMigratorHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DbMigratorHostedService> _logger;

        public DbMigratorHostedService(IServiceProvider serviceProvider, ILogger<DbMigratorHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Database migration starting.");
            using var scope = _serviceProvider.CreateScope();
            
            var dbContext = scope.ServiceProvider.GetRequiredService<IBaseDbContext>() as DbContext;
            await dbContext.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migration finished.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}