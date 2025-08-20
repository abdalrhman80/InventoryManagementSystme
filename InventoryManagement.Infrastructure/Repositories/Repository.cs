using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications;
using InventoryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Repositories
{
    internal class Repository<T>(ApplicationDbContext _dbContext) : IRepository<T> where T : class
    {
        #region Without Specification
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
        #endregion

        #region With Specification
        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecificationAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);
        }
        #endregion
    }
}
