using InventoryManagement.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace InventoryManagement.Application.UserContextService
{
    public class UserContext(IHttpContextAccessor _httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("User context is not present.");

            if (user.Identity is null || !user.Identity.IsAuthenticated)
                return null;

            var id = user.FindFirst(c => c.Type == JwtClaimsTypes.UserId)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var userName = user.FindFirst(c => c.Type == JwtClaimsTypes.UserName)!.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);

            return new CurrentUser(id, userName, email, roles);
        }
    }
}
