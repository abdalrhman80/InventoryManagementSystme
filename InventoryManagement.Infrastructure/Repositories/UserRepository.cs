using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.Infrastructure.Repositories
{
    internal class UserRepository(
        ApplicationDbContext _context,
        UserManager<User> _userManager
        ) : Repository<User>(_context), IUserRepository
    {
        public async Task<User?> GetByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task UpdateAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException("Failed to update user");
        }

        public async Task DeleteAsync(User user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new BadRequestException("Failed to delete user");
        }

        public async Task ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            //if (!result.Succeeded)
            //    throw new BadRequestException("Failed to change password");
        }
    }
}
