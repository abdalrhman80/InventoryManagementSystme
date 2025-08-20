using MediatR;

namespace InventoryManagement.Application.Account.Commands.RemoveUserFromRole
{
    public record RemoveUserFromRoleCommand(string UserId, string RoleName) : IRequest;
}
