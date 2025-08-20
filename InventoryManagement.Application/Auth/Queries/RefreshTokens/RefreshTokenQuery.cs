using InventoryManagement.Domain.Common;
using MediatR;

namespace InventoryManagement.Application.Auth.Queries.RefreshTokens
{
    public record RefreshTokenQuery(string Token) : IRequest<AuthenticationResponse>;
}
