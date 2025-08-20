using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Auth.Commands.ResendConfirmation
{
    public class ResendConfirmationCommandHandler(
        ILogger<ResendConfirmationCommandHandler> _logger,
        UserManager<User> _userManager,
        IAuthService _authService
        ) : IRequestHandler<ResendConfirmationCommand, string>
    {
        public async Task<string> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new BadRequestException("Invalid user email.");

            if (user.EmailConfirmed)
                throw new BadRequestException("Email is already confirmed.");

            await _authService.SendCodeConfirmationEmailAsync(user);

            _logger.LogInformation("User {UserId} requested to resend email confirmation code", user.Id);

            return "Confirmation email has been resent.";
        }
    }
}
