using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        void AddTransactionAsync(Transaction transaction);
    }
}
