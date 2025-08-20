using InventoryManagement.Domain.Common;
using MediatR;

namespace InventoryManagement.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthenticationResponse>;
}
