using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace InventoryManagement.Infrastructure.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly Hashtable _repositories;
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction;
        public IUserRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ITransactionRepository TransactionRepository { get; }

        public UnitOfWork(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            _repositories = [];
            _dbContext = dbContext;
            UserRepository = new UserRepository(_dbContext, userManager);
            ProductRepository = new ProductRepository(_dbContext);
            TransactionRepository = new TransactionRepository(_dbContext);
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new Repository<T>(_dbContext);
                _repositories.Add(type, repository);
            }

            return (IRepository<T>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Transaction has already been started.");
            }

            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            try
            {
                return _transaction.RollbackAsync();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
