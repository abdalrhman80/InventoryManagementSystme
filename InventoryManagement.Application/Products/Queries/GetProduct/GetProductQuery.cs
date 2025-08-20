using InventoryManagement.Application.Products.DTOs;
using MediatR;

namespace InventoryManagement.Application.Products.Queries.GetProduct
{
    public record GetProductQuery(int Id) : IRequest<ProductDto>;
}
