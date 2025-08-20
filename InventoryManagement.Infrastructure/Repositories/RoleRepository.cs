using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Repositories
{
    internal class RoleRepository(ApplicationDbContext _context) : Repository<Role>(_context), IRoleRepository
    {
        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
                ?? throw new InvalidOperationException(message: $"Role {roleName} does not exist");

            var existingUserRole = await _context.Set<UserRole>()
                .FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

            if (existingUserRole != null)
                throw new InvalidOperationException($"User {user.Id} is already in role {roleName}");

            // Create custom UserRole entry
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
                User = user,
                Role = role
            };

            _context.Set<UserRole>().Add(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromRoleAsync(User user, string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName) ??
                throw new InvalidOperationException(message: $"Role {roleName} does not exist");

            var userRole = await _context.Set<UserRole>()
                .FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

            if (userRole != null)
            {
                _context.Set<UserRole>().Remove(userRole);
                await _context.SaveChangesAsync();
            }
        }
    }
}
