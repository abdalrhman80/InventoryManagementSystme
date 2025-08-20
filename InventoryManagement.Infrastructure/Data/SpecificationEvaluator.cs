using InventoryManagement.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Data
{
    internal class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            // Apply criteria
            if (specification.Criteria != null)
                query = query.Where(specification.Criteria);

            // Includes all expression-based includes
            if (specification.Includes != null && specification.Includes.Count > 0)
                query = specification.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            // Include any string-based include statements
            if (specification.IncludeStrings != null && specification.IncludeStrings.Count > 0)
                query = specification.IncludeStrings.Aggregate(query, (currentQuery, includeString) => currentQuery.Include(includeString));

            // Apply ordering
            if (specification.OrderBy != null)
                query = query.OrderBy(specification.OrderBy).AsQueryable();

            if (specification.OrderByDescending != null)
                query = query.OrderByDescending(specification.OrderByDescending).AsQueryable();

            // Apply grouping
            if (specification.GroupBy != null)
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);

            // Apply pagination
            if (specification.IsPagingEnabled)
                query = query.Skip(specification.Skip).Take(specification.Take);

            return query;
        }
    }
}
