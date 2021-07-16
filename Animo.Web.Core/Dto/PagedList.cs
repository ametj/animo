using System;
using System.Collections.Generic;

namespace Animo.Web.Core.Dto
{
    public enum SortMode { Asc, Desc }

    public record PagedList<T>(
        int Page,
        List<T> Results,
        int TotalPages,
        int TotalResults)
    {
        public PagedList(List<T> userListOutput, int usersCount, PagedListRequest request) : this(request.PageIndex, userListOutput, (int)Math.Ceiling(usersCount / (decimal)request.PageSize), usersCount)
        {
        }
    }

    public record PagedListRequest(
        int PageIndex = 0,
        int PageSize = 10,
        string Filter = null,
        string SortBy = null);
}