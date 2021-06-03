using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Animo.Web.Tests.Integration
{
    [Collection("CustomWebApplicationFactory")]
    public class TestBase
    {
        protected CustomWebApplicationFactory _factory;
        protected HttpClient _client;

        public TestBase(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _client.GetAsync(requestUri);
        }

        protected async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await _client.PostAsync(requestUri, content);
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T value)
        {
            return await _client.PostAsJsonAsync(requestUri, value);
        }

        protected async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            return await _client.PutAsync(requestUri, content);
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string requestUri, T value)
        {
            return await _client.PutAsJsonAsync(requestUri, value);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await _client.DeleteAsync(requestUri);
        }
    }
}