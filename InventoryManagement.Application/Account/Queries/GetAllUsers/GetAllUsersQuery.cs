using InventoryManagement.Application.Account.DTOs;
using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Constants;
using MediatR;

namespace InventoryManagement.Application.Account.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<PaginationResponse<UserDto>>
    {
        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 10 ? 10 : value;
        }

        public int PageNumber { get; set; } = 1;
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}
