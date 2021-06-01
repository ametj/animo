using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Animo.Web.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>(
            this IQueryable<TEntity> source,
            bool condition = false,
            Expression<Func<TEntity, bool>> predicate = null)
        {
            if (condition && predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(
            this IQueryable<TEntity> source,
            string sortBy,
            string defaultSortBy)
        {
            var orderBy = !sortBy.IsNullOrEmpty() ? sortBy : defaultSortBy;

            return orderBy.IsNullOrEmpty() ? source : source.OrderBy(orderBy);
        }

        public static IQueryable<T> PagedBy<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize,
            int indexFrom = 0)
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            return source.Skip((pageIndex - indexFrom) * pageSize).Take(pageSize);
        }
    }
}