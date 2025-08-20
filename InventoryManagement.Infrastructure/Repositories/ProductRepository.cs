using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Repositories
{
    internal class ProductRepository(ApplicationDbContext _context) : Repository<Product>(_context), IProductRepository
    {
        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IReadOnlyList<Product>?> GetByCategoryAsync(string categoryName)
        {
            return await _context.Products.Where(p => p.Category.Name == categoryName).AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<Product>?> GetBySupplierAsync(string supplier)
        {
            return await _context.Products.Where(p => p.Supplier == supplier).AsNoTracking().ToListAsync();
        }
    }
}
