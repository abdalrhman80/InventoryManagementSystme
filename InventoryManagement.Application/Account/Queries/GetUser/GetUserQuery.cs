using InventoryManagement.Application.Account.DTOs;
using MediatR;

namespace InventoryManagement.Application.Account.Queries.GetUser
{
    public class GetUserQuery: IRequest<UserDto>
    {
    }
}
