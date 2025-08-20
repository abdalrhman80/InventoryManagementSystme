using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;

namespace InventoryManagement.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByIdAsync(string userId);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}
