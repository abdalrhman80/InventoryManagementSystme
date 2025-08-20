using MediatR;

namespace InventoryManagement.Application.Auth.Commands.ForgetPassword
{
    public record ForgetPasswordCommand(string Email) : IRequest<string>;
}
