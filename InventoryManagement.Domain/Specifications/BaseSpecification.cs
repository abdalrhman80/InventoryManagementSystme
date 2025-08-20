using System.Linq.Expressions;

namespace InventoryManagement.Domain.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class
    {
        protected BaseSpecification() { }

        // BaseSpecification(Expression<Func<T, bool>> criteria) => Criteria = criteria;

        public Expression<Func<T, bool>> Criteria { get; private set; }
        public List<Expression<Func<T, object>>> Includes { get; } = [];
        public List<string> IncludeStrings { get; } = [];
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;

        protected void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddIncludes(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddIncludes(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

        protected void ApplyPagination(int skip, int take)
        {
            IsPagingEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
