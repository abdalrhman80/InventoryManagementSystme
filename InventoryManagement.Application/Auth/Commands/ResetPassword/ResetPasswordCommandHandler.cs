using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(
        ILogger<ResetPasswordCommandHandler> _logger,
        UserManager<User> _userManager
        )
        : IRequestHandler<ResetPasswordCommand, string>
    {
        public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new BadRequestException("Email not exists");

            if (user is null || user.PasswordResetCode != request.PasswordResetCode || user.PasswordResetCodeExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Invalid or expired reset code.");

            var result = await _userManager.ResetPasswordAsync(user, user.PasswordResetToken!, request.NewPassword);

            if (!result.Succeeded)
                throw new BadRequestException(string.Join(", ", result.Errors.Select(e => e.Description)));

            _logger.LogInformation("Password reset successfully for user {UserId}", user.Id);

            user.PasswordResetCode = null;
            user.PasswordResetToken = null;
            user.PasswordResetCodeExpiresAt = null;
            await _userManager.UpdateAsync(user);

            return "Password has been reset successfully.";
        }
    }
}
