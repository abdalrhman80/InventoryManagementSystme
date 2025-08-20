namespace InventoryManagement.Domain.Common
{
    public record AuthenticationResponse(
           string Email,
           string UserName,
           List<string> Roles,
           string AccessToken,
           DateTime TokenExpiration,
           string RefreshToken,
           DateTime RefreshTokenExpiration);
}
