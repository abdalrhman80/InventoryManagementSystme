using InventoryManagement.Application.Categories.DTOs;
using InventoryManagement.Domain.Common;
using MediatR;

namespace InventoryManagement.Application.Categories.Query.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<PaginationResponse<CategoryDto>>
    {
        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 10 ? 10 : value;
        }
        public int PageNumber { get; set; } = 1;
    }
}
