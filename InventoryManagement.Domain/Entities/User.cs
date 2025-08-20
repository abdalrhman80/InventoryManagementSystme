using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        [MaxLength(100)]
        public string? PhoneNumber { get; set; }
        [MaxLength(255)]
        public string? ImagePath { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpiresAt { get; set; }
        public string? PasswordResetCode { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetCodeExpiresAt { get; set; }

        public ICollection<RefreshToken>? RefreshTokens { get; set; } = [];
        public ICollection<Transaction> Transactions { get; set; } = [];
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
