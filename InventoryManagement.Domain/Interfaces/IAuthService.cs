using InventoryManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagement.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> GenerateAccessTokenAsync(User user);
        RefreshToken GenerateRefreshToken();
        Task SendCodeConfirmationEmailAsync(User user);
        Task SendPasswordResetCodeAsync(User user);
    }
}
