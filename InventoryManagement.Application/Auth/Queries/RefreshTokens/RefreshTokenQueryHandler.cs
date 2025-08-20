using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagement.Application.Auth.Queries.RefreshTokens
{
    public class RefreshTokenQueryHandler(
        ILogger<RefreshTokenQueryHandler> _logger,
         UserManager<User> _userManager,
         IAuthService _authService
        ) : IRequestHandler<RefreshTokenQuery, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token))
                throw new BadRequestException("Token is required!");

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == request.Token), cancellationToken: cancellationToken)
                ?? throw new BadRequestException("Invalid Refresh Token");

            if (!user.EmailConfirmed)
                throw new BadRequestException("Please confirm your email before accessing the application.");

            var refreshToken = user.RefreshTokens!.Single(t => t.Token == request.Token);

            if (!refreshToken.IsActive)
                throw new BadRequestException("Invalid Refresh Token");

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = _authService.GenerateRefreshToken();

            user.RefreshTokens!.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} refreshed their token successfully.", user.Id);

            var jwtSecurityToken = await _authService.GenerateAccessTokenAsync(user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var userRoles = jwtSecurityToken.Claims.Where(c => c.Type == JwtClaimsTypes.Role).Select(c => c.Value).ToList();

            var response = new AuthenticationResponse(
                user.Email!,
                user.UserName!,
                userRoles,
                accessToken,
                jwtSecurityToken.ValidTo,
                newRefreshToken.Token,
                newRefreshToken.ExpirationOn.ToLocalTime());

            return response;
        }
    }
}
