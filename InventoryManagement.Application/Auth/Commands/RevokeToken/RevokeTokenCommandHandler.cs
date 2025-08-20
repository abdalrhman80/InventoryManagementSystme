using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler(
        ILogger<RevokeTokenCommandHandler> _logger,
        UserManager<User> _userManager) 
        : IRequestHandler<RevokeTokenCommand, string>
    {
        public async Task<string> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
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

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User {UserId} revoked their token successfully.", user.Id);

            return "Token revoked successfully.";
        }
    }

}
