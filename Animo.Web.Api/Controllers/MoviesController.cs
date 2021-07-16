using Animo.Web.Api.Dto;
using Animo.Web.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace Animo.Web.Api.Controllers
{
    public class MoviesController : BaseController
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Movie(int id)
        {
            var movie = await _movieService.GetMovieAsync(id);
            return Ok(movie);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SearchContainer<SearchItem>>> Popular()
        {
            var movies = await _movieService.GetMoviePopularListAsync();
            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SearchContainer<SearchItem>>> TopRated()
        {
            var movies = await _movieService.GetMovieTopRatedListAsync();
            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SearchContainer<SearchItem>>> Upcoming()
        {
            var movies = await _movieService.GetMovieUpcomingListAsync();
            return Ok(movies);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<SearchContainer<SearchItem>>> NowPlaying()
        {
            var movies = await _movieService.GetMovieNowPlayingListAsync();
            return Ok(movies);
        }
    }
}