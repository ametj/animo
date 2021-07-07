using Animo.Web.Core.Services;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Animo.Web.Api.Services
{
    public class TMDBClientFactoryInitializationHostedService : IHostedService
    {
        private readonly ITMDbClientFactory _tmdbClientFactory;

        public TMDBClientFactoryInitializationHostedService(ITMDbClientFactory tmdbClientFactory)
        {
            _tmdbClientFactory = tmdbClientFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _tmdbClientFactory.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}