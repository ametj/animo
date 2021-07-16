using Animo.Web.Api.Dto;
using AutoMapper;
using TMDbLib.Objects.Search;

namespace Animo.Web.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SearchMovie, SearchItem>()
                .ForCtorParam(nameof(SearchItem.TmdbId), c => c.MapFrom(s => s.Id))
                .ForCtorParam(nameof(SearchItem.Name), c => c.MapFrom(s => s.Title))
                .ForCtorParam(nameof(SearchItem.Released), c => c.MapFrom(s => s.ReleaseDate));

            CreateMap<SearchTv, SearchItem>()
                .ForCtorParam(nameof(SearchItem.TmdbId), c => c.MapFrom(s => s.Id))
                .ForCtorParam(nameof(SearchItem.Released), c => c.MapFrom(s => s.FirstAirDate));
        }
    }
}