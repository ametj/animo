using System;
using TMDbLib.Client;

namespace Animo.Web.Core.Services
{
    public class TMDbService : ITMDbService, IDisposable
    {
        private readonly TMDbClient _client;

        public TMDbService(ITMDbClientFactory clientFactory)
        {
            _client = clientFactory.Create();
        }

        public TMDbClient Client => _client;

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}