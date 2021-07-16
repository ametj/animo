using Animo.Web.Api.Dto;
using Animo.Web.Core.Dto;
using Animo.Web.Core.Services;
using AutoMapper;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace Animo.Web.Api.Services.TMDB
{
    public class MovieService : IMovieService
    {
        private readonly ITMDbService _tmdbService;
        private readonly IMapper _mapper;

        public MovieService(ITMDbService tmdbService, IMapper mapper)
        {
            _tmdbService = tmdbService;
            _mapper = mapper;
        }

        public Task<Movie> GetMovieAsync(int id)
        {
            return _tmdbService.Client.GetMovieAsync(id);
        }

        public async Task<PagedList<SearchItem>> GetMovieNowPlayingListAsync()
        {
            var movies = await _tmdbService.Client.GetMovieNowPlayingListAsync();
            return _mapper.Map<PagedList<SearchItem>>(movies);
        }

        public async Task<PagedList<SearchItem>> GetMoviePopularListAsync()
        {
            var movies = await _tmdbService.Client.GetMoviePopularListAsync();
            return _mapper.Map<PagedList<SearchItem>>(movies);
        }

        public async Task<PagedList<SearchItem>> GetMovieTopRatedListAsync()
        {
            var movies = await _tmdbService.Client.GetMovieTopRatedListAsync();
            return _mapper.Map<PagedList<SearchItem>>(movies);
        }

        public async Task<PagedList<SearchItem>> GetMovieUpcomingListAsync()
        {
            var movies = await _tmdbService.Client.GetMovieUpcomingListAsync();
            return _mapper.Map<PagedList<SearchItem>>(movies);
        }
    }
}