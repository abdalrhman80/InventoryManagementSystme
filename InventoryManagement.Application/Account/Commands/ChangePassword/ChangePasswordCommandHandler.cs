using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler(
        ILogger<ChangePasswordCommandHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository
        ) : IRequestHandler<ChangePasswordCommand>
    {
        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbUser = await _userRepository.GetByIdAsync(user.Id) ?? throw new NotFoundException(message: $"User with ID {user.Id} not found.");

            _logger.LogInformation("Changing password for user: {UserId}", dbUser.Id);

            await _userRepository.ChangePasswordAsync(dbUser, request.CurrentPassword, request.NewPassword);
        }
    }
}
