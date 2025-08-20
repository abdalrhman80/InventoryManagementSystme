using InventoryManagement.Application.Account.DTOs;
using MediatR;

namespace InventoryManagement.Application.Account.Commands.UpdateUser
{
    public record UpdateUserCommand(string? Email, string? FirstName, string? LastName, string? PhoneNumber, string? Address)
        : IRequest<UserDto>;
}
