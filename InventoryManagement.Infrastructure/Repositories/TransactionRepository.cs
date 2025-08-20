using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Infrastructure.Data;

namespace InventoryManagement.Infrastructure.Repositories
{
    internal class TransactionRepository(ApplicationDbContext dbContext) : Repository<Transaction>(dbContext), ITransactionRepository
    {
        public void AddTransactionAsync(Transaction transaction)
        {
            dbContext.Transactions.Add(transaction);
        }
    }
}
