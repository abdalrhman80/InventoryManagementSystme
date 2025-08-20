using MediatR;

namespace InventoryManagement.Application.Auth.Commands.RevokeToken
{
    public record RevokeTokenCommand(string Token) : IRequest<string>;
}
