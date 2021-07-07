using Animo.Web.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Animo.Web.Api.Controllers
{
    public class TMDBController : BaseController
    {
        private readonly ITMDbService _tmdbService;

        public TMDBController(ITMDbService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> Movie(int id)
        {
            var client = _tmdbService.Client;

            var movie = await client.GetMovieAsync(id);
            return Ok(movie);
        }
    }
}