using Animo.Web.Api;
using Animo.Web.Api.Dto;
using AutoMapper;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;
using Xunit;

namespace Animo.Web.Tests.Unit
{
    public class MapperTests : TestBase
    {
        private readonly Mapper _mapper;

        public MapperTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task SearchMovieToSearchItemMappingTest()
        {
            var searchMovie = await LoadJsonAsync<SearchMovie>("./Data/SearchMovie.json");

            var searchItem = _mapper.Map<SearchItem>(searchMovie);

            Assert.Equal(searchMovie.Id, searchItem.TmdbId);
            Assert.Equal(searchMovie.Title, searchItem.Name);
            Assert.Equal(searchMovie.MediaType, searchItem.MediaType);
            Assert.Equal(searchMovie.BackdropPath, searchItem.BackdropPath);
            Assert.Equal(searchMovie.PosterPath, searchItem.PosterPath);
            Assert.Equal(searchMovie.ReleaseDate, searchItem.Released);
        }

        [Fact]
        public async Task SearchTvToSearchItemMappingTest()
        {
            var searchTv = await LoadJsonAsync<SearchTv>("./Data/SearchTv.json");

            var searchItem = _mapper.Map<SearchItem>(searchTv);

            Assert.Equal(searchTv.Id, searchItem.TmdbId);
            Assert.Equal(searchTv.Name, searchItem.Name);
            Assert.Equal(searchTv.MediaType, searchItem.MediaType);
            Assert.Equal(searchTv.BackdropPath, searchItem.BackdropPath);
            Assert.Equal(searchTv.PosterPath, searchItem.PosterPath);
            Assert.Equal(searchTv.FirstAirDate, searchItem.Released);
        }
    }
}