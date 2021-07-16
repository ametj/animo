using System;
using TMDbLib.Objects.General;

namespace Animo.Web.Api.Dto
{
    public record SearchItem(
        int TmdbId,
        string Name,
        MediaType MediaType,
        DateTime? Released,
        string BackdropPath,
        string PosterPath
    );
}