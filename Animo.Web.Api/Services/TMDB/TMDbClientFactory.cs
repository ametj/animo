using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;

namespace Animo.Web.Core.Services
{
    public class TMDbClientFactory : ITMDbClientFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TMDbClientFactory> _logger;
        private TMDbConfig _config;

        public TMDbClientFactory(IConfiguration configuration, ILogger<TMDbClientFactory> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Initialization starting.");
            var client = CreateTMDbClient();
            _config = await client.GetConfigAsync();
            _logger.LogInformation("TMDbClient config loaded.");
        }

        public TMDbClient Create()
        {
            var client = CreateTMDbClient();

            if (_config != null)
            {
                client.SetConfig(_config);
            }

            return client;
        }

        private TMDbClient CreateTMDbClient()
        {
            return new TMDbClient(_configuration["ExternalApiKeys:TMDB"]);
        }
    }
}