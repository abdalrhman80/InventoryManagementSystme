using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Auth.Commands.ForgetPassword
{
    public class ForgetPasswordCommandHandler(
        ILogger<ForgetPasswordCommandHandler> _logger,
        UserManager<User> _userManager,
        IAuthService _authService
        )
        : IRequestHandler<ForgetPasswordCommand, string>
    {
        public async Task<string> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)?? throw new BadRequestException("Email not exists");

            if(!user.EmailConfirmed)
                throw new BadRequestException("Email not confirmed");

            await _authService.SendPasswordResetCodeAsync(user);

            _logger.LogInformation("Password reset code sent to {Email}", request.Email);

            return "Password reset code sent successfully to your email";
        }
    }
}
