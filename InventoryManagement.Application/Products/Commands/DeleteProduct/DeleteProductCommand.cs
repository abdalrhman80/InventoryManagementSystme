using MediatR;

namespace InventoryManagement.Application.Products.Commands.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest;
}
