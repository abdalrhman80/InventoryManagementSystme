using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Entities;
using System.Linq.Expressions;

namespace InventoryManagement.Domain.Specifications.EntitiesSpecifications
{
    public class LowStockAlertSpecification : BaseSpecification<LowStockAlert>
    {
        public LowStockAlertSpecification(int? id = null, int? productId = null, bool includeProduct = false)
        {
            var predicates = new List<Expression<Func<LowStockAlert, bool>>>();

            if (id.HasValue)
                predicates.Add(a => a.Id == id.Value);

            if (productId.HasValue)
                predicates.Add(a => a.ProductId == productId.Value);

            if (predicates.Count > 0)
            {
                AddCriteria(predicates.Aggregate((current, next) => current.And(next)));
            }

            if (includeProduct)
                AddIncludes(a => a.Product);
        }
    }
}
