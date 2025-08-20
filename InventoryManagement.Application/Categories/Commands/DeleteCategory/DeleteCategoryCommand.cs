using MediatR;

namespace InventoryManagement.Application.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int CategoryId) : IRequest;
}
