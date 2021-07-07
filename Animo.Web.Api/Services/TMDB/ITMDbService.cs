using TMDbLib.Client;

namespace Animo.Web.Core.Services
{
    public interface ITMDbService
    {
        TMDbClient Client { get; }
    }
}