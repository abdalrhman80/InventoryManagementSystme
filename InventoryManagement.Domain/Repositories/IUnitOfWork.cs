namespace InventoryManagement.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        IUserRepository UserRepository { get; }
        IProductRepository ProductRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
