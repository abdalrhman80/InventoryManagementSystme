using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Auth.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler(
        ILogger<ConfirmEmailCommandHandler> _logger,
        UserManager<User> _userManager
        ) : IRequestHandler<ConfirmEmailCommand, string>
    {
        public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.ConfirmationCode))
                throw new BadRequestException("Email and code are required");

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || user.EmailConfirmationCode != request.ConfirmationCode || user.EmailConfirmationCodeExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Invalid or expired confirmation code.");

            if (user.EmailConfirmed)
                throw new BadRequestException("Email is already confirmed.");

            var confirmation = await _userManager.ConfirmEmailAsync(user, user.EmailConfirmationToken!);

            if (!confirmation.Succeeded)
                throw new BadRequestException("Confirmation not succeeded.");

            user.EmailConfirmationCode = null;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationCodeExpiresAt = null;
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Email {Email} confirmed successfully for user {UserId}", request.Email, user.Id);

            return "Email confirmed successfully. You can now login.";
        }
    }
}
