using MediatR;

namespace InventoryManagement.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price, int StockQuantity, string Supplier, int CategoryId) : IRequest<int>;
}
