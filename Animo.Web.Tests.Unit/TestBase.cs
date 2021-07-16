using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Animo.Web.Tests.Unit
{
    public abstract class TestBase
    {
        protected JsonSerializerOptions _options;

        public TestBase()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        protected async Task<T> LoadJsonAsync<T>(string path)
        {
            var file = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<T>(file, _options);
        }
    }
}