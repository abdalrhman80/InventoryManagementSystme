using MediatR;

namespace InventoryManagement.Application.Auth.Commands.Register
{
    public record RegisterCommand(string FirstName, string LastName, string UserName, string Email, string Password) 
        : IRequest<string>;
}
