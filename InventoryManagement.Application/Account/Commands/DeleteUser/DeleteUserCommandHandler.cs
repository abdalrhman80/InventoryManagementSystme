using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(
        ILogger<DeleteUserCommandHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository) 
        : IRequestHandler<DeleteUserCommand>
    {
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            var dbUser = await _userRepository.GetByIdAsync(user.Id) ?? throw new NotFoundException(message: $"User with ID {user.Id} not found.");

            _logger.LogInformation("Deleting user: {UserId}", dbUser.Id);

            await _userRepository.DeleteAsync(dbUser);
        }
    }
}
