using MediatR;

namespace InventoryManagement.Application.Account.Commands.AddUserToRole
{
    public record AddUserToRoleCommand(string UserId, string RoleName) : IRequest;
}
