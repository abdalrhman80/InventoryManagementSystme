using MediatR;

namespace InventoryManagement.Application.Auth.Commands.ResendConfirmation
{
    public record ResendConfirmationCommand(string Email) : IRequest<string>;
}
