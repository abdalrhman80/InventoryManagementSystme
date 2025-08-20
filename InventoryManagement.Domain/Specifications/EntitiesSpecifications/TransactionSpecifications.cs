using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace InventoryManagement.Domain.Specifications.EntitiesSpecifications
{
    public class TransactionSpecifications : BaseSpecification<Transaction>
    {
        public TransactionSpecifications(int id)
        {
            AddCriteria(c => c.Id == id);
            AddIncludes(t => t.User);
            AddIncludes(t => t.Product);
        }

        public TransactionSpecifications(
            int pageSize,
            int pageNumber,
            string? sortBy,
            SortDirection sortDirection,
            TransactionType? transactionType = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            ApplyFilters(transactionType, startDate, endDate);
            AddIncludes(t => t.User);
            AddIncludes(t => t.Product);
            ApplySorting(sortBy, sortDirection);
            ApplyPagination(pageSize * (pageNumber - 1), pageSize);
        }

        private void ApplyFilters(TransactionType? transactionType, DateTime? startDate, DateTime? endDate)
        {
            var predicates = new List<Expression<Func<Transaction, bool>>>();

            if (transactionType.HasValue)
                predicates.Add(t => t.Type == transactionType.Value);

            predicates.Add(DateRangeFilter(startDate, endDate));

            if (predicates.Count > 0)
            {
                AddCriteria(predicates.Aggregate((current, next) => current.And(next)));
            }
            else
            {
                AddCriteria(t => true);
            }
        }

        private static Expression<Func<Transaction, bool>> DateRangeFilter(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1);
                return t => t.CreateDate >= startDate.Value && t.CreateDate <= endOfDay;
            }
            else if (startDate.HasValue)
            {
                return t => t.CreateDate >= startDate.Value;
            }
            else if (endDate.HasValue)
            {
                var endOfDay = endDate.Value.Date.AddDays(1).AddTicks(-1);
                return t => t.CreateDate <= endOfDay;
            }

            return t => true;
        }

        private void ApplySorting(string? sortBy, SortDirection sortDirection)
        {
            var key = sortBy?.Trim().ToLowerInvariant();

            var sortSelectors = new Dictionary<string, Expression<Func<Transaction, object>>>
            {
                [nameof(Transaction.Id).ToLower()] = x => x.Id,
                [nameof(Transaction.CreateDate).ToLower()] = x => x.CreateDate,
                [nameof(Transaction.TotalAmount).ToLower()] = x => x.TotalAmount,
                [nameof(Transaction.Quantity).ToLower()] = x => x.Quantity!,
            };

            if (string.IsNullOrEmpty(key) || !sortSelectors.ContainsKey(key))
                key = nameof(Transaction.Id).ToLower();

            var selector = sortSelectors[key];

            if (sortDirection == SortDirection.Descending)
                ApplyOrderByDescending(selector);
            else
                ApplyOrderBy(selector);
        }
    }
}
