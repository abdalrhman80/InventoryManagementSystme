using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task AddUserToRoleAsync(User user, string roleName);
        Task RemoveUserFromRoleAsync(User user, string roleName);
    }
}
