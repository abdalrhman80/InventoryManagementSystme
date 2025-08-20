using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.DatabaseInitializer
{
    internal class DbInitializer(
        ApplicationDbContext _dbContext,
        UserManager<User> _userManager,
        RoleManager<Role> _roleManager,
        IRoleRepository _roleRepository
        ) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            #region Update Database (Migrations)
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                    await _dbContext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An Error Occurred During Appling The Migrations", ex);
            }
            #endregion

            #region Create Roles and Admin User
            if (!await _roleManager.RoleExistsAsync(RoleNames.Admin))
            {
                _dbContext.Roles.AddRange(UserRoles);
                await _dbContext.SaveChangesAsync();

                var newAdmin = new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "Administrator",
                    LastName = "Admin",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(newAdmin, "P@$$w0rd1234");

                var administrator = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == newAdmin.Email);

                await _roleRepository.AddUserToRoleAsync(administrator!, RoleNames.Admin);
            }
            #endregion
        }

        private static IEnumerable<Role> UserRoles
        {
            get
            {
                List<Role> roles =
                [
                    new(RoleNames.Admin),
                    new(RoleNames.Manager),
                    new(RoleNames.Staff),
                ];

                return roles;
            }
        }
    }
}
