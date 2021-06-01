using System.Collections.Generic;

namespace Animo.Web.Core.Dto
{
    public enum SortMode { Asc, Desc }

    public record PagedList<T>(IList<T> Items, int TotalCount);

    public record PagedListRequest(
        int PageIndex = 0,
        int PageSize = 10,
        string Filter = null,
        string SortBy = null);
}