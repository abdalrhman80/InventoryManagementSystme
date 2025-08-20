using InventoryManagement.Domain.Common;
using MediatR;

namespace InventoryManagement.Application.Auth.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Email, string ConfirmationCode) : IRequest<string>;
}
