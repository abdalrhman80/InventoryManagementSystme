using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using System.Linq.Expressions;

namespace InventoryManagement.Domain.Specifications.EntitiesSpecifications
{
    public class ProductSpecifications : BaseSpecification<Product>
    {
        public ProductSpecifications(int id)
        {
            AddIncludes(p => p.Category);
            AddCriteria(p => p.Id == id);
        }

        public ProductSpecifications(int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection, string? categoryName = null)
        {
            ApplySorting(sortBy, sortDirection);

            AddIncludes(p => p.Category);

            ApplyPagination(pageSize * (pageNumber - 1), pageSize);

            if (!string.IsNullOrEmpty(categoryName))
                AddCriteria(x => x.Category.Name.Contains(categoryName));
        }

        private void ApplySorting(string? sortBy, SortDirection sortDirection)
        {
            var key = sortBy?.Trim().ToLowerInvariant();

            var sortSelectors = new Dictionary<string, Expression<Func<Product, object>>>
            {
                [nameof(Product.Id).ToLower()] = x => x.Id,
                [nameof(Product.Name).ToLower()] = x => x.Name,
                [nameof(Product.Price).ToLower()] = x => x.Price,
                [nameof(Product.CreatedAt).ToLower()] = x => x.CreatedAt!,
                [nameof(Product.StockQuantity).ToLower()] = x => x.StockQuantity!
            };

            if (string.IsNullOrEmpty(key) || !sortSelectors.ContainsKey(key))
                key = nameof(Product.Id).ToLower();

            var selector = sortSelectors[key];

            if (sortDirection == SortDirection.Descending)
                ApplyOrderByDescending(selector);
            else
                ApplyOrderBy(selector);
        }

        private void ApplyFilters(int? categoryId, string? supplier)
        {
            var predicates = new List<Expression<Func<Product, bool>>>();

            if (categoryId != null)
                predicates.Add(p => p.CategoryId == categoryId);

            if (supplier != null)
                predicates.Add(p => p.Supplier.Contains(supplier));

            if (predicates.Count > 0)
            {
                AddCriteria(predicates.Aggregate((current, next) => current.And(next)));
            }
            else
            {
                AddCriteria(t => true);
            }
        }

    }
}
