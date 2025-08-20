using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Specifications.EntitiesSpecifications
{
    public class CategoriesSpecifications : BaseSpecification<Category>
    {
        public CategoriesSpecifications(int pageSize, int pageNumber)
        {
            ApplyPagination(pageSize * (pageNumber - 1), pageSize);
        }

        public CategoriesSpecifications(int categoryId)
        {
            AddCriteria(category => category.Id == categoryId);
            AddIncludes(c => c.Products);
        }
    }
}
