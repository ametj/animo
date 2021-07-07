using System.Threading.Tasks;
using TMDbLib.Client;

namespace Animo.Web.Core.Services
{
    public interface ITMDbClientFactory
    {
        TMDbClient Create();

        Task InitializeAsync();
    }
}