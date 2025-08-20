using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Account.Commands.RemoveUserFromRole
{
    public class RemoveUserFromRoleCommandHandler(
        ILogger<RemoveUserFromRoleCommandHandler> _logger,
        IUserContext _userContext,
        IUserRepository _userRepository,
        IRoleRepository _roleRepository
        ) : IRequestHandler<RemoveUserFromRoleCommand>
    {
        public async Task Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
        {
            var adminUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation(
                "Admin {AdminId} is removing user {UserIdToRemove} from role {RoleName}",
                adminUser.Id, request.UserId, request.RoleName
            );

            if (string.IsNullOrEmpty(request.UserId))
                throw new BadRequestException("User ID is required.");

            if (string.IsNullOrEmpty(request.RoleName))
                throw new BadRequestException("Role Name is required.");

            var user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new NotFoundException(message: "User not found");

            await _roleRepository.RemoveUserFromRoleAsync(user, request.RoleName);
        }
    }
}
