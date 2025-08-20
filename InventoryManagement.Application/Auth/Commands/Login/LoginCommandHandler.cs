using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagement.Application.Auth.Commands.Login
{
    public class LoginCommandHandler(
        ILogger<LoginCommandHandler> _logger,
        IAuthService _authService,
        UserManager<User> _userManager)
        : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new BadRequestException("Email or password is not valid.");

            if (!user.EmailConfirmed)
                throw new BadRequestException("Please confirm your email before logging in.");

            _logger.LogInformation("User {UserId} logged in successfully with email", user.Id);

            var jwtSecurityToken = await _authService.GenerateAccessTokenAsync(user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            var userRoles = jwtSecurityToken.Claims.Where(c => c.Type == JwtClaimsTypes.Role).Select(c => c.Value).ToList();

            // Generate a new refresh token
            string refreshToken = string.Empty;
            DateTime refreshTokenExpiration = DateTime.UtcNow;

            if (user.RefreshTokens!.Any(u => u.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens!.FirstOrDefault(u => u.IsActive);
                refreshToken = activeRefreshToken!.Token;
                refreshTokenExpiration = activeRefreshToken.ExpirationOn.ToLocalTime();
            }
            else
            {
                var newRefreshToken = _authService.GenerateRefreshToken();

                user.RefreshTokens!.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);

                refreshToken = newRefreshToken.Token;
                refreshTokenExpiration = newRefreshToken.ExpirationOn.ToLocalTime();
            }

            var response = new AuthenticationResponse(
                user.Email!,
                user.UserName!,
                userRoles,
                accessToken,
                jwtSecurityToken.ValidTo,
                refreshToken,
                refreshTokenExpiration);

            return response;
        }
    }
}
