using Animo.Web.Api.Dto;
using Animo.Web.Core.Dto;
using System.Threading.Tasks;
using TMDbLib.Objects.Movies;

namespace Animo.Web.Api.Services
{
    public interface IMovieService
    {
        Task<Movie> GetMovieAsync(int id);

        Task<PagedList<SearchItem>> GetMoviePopularListAsync();

        Task<PagedList<SearchItem>> GetMovieTopRatedListAsync();

        Task<PagedList<SearchItem>> GetMovieUpcomingListAsync();

        Task<PagedList<SearchItem>> GetMovieNowPlayingListAsync();
    }
}