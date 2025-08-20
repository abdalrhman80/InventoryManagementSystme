using InventoryManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Data
{
    internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, string>(options)
    {
        internal DbSet<User> Users { get; set; }
        internal DbSet<Product> Products { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<Transaction> Transactions { get; set; }
        internal DbSet<LowStockAlert> LowStockAlerts { get; set; }
        internal DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(t => t.Product)
                .WithMany(p => p.Transactions)
                .HasForeignKey(t => t.ProductId);

                entity.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.CreatedBy);

                entity.Property(t => t.Type)
                .HasConversion<string>();
            });

            modelBuilder.Entity<LowStockAlert>(entity =>
            {
                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("Users");

                user.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(role =>
            {
                role.ToTable("Roles");

                role.HasMany(r => r.UserRoles)
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<IdentityUserRole<string>>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<UserRole>(userRole =>
            {
                //userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.ToTable("UserRoles");

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(userClaim =>
            {
                userClaim.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(userLogin =>
            {
                userLogin.ToTable("UserLogins").HasKey(l => new { l.LoginProvider, l.ProviderKey });
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(roleClaim =>
            {
                roleClaim.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(userToken =>
            {
                userToken.ToTable("UserTokens").HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            });
        }
    }
}
