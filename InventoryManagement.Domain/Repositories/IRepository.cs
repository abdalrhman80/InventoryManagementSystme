using InventoryManagement.Domain.Specifications;

namespace InventoryManagement.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        #region Without Specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        #endregion

        #region With Specifications
        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification);
        Task<T?> GetEntityWithSpecificationAsync(ISpecification<T> specification);
        #endregion
    }
}
