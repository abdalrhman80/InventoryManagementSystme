using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product?> GetByNameAsync(string name);
        Task<IReadOnlyList<Product>?> GetByCategoryAsync(string categoryName);
        Task<IReadOnlyList<Product>?> GetBySupplierAsync(string supplier);
    }
}
