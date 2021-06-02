using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Animo.Web.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static StringContent ToStringConent(this object source)
        {
            var json = JsonSerializer.Serialize(source);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}