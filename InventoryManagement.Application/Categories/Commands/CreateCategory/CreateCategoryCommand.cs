using MediatR;

namespace InventoryManagement.Application.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(string CategoryName, string? Description) : IRequest<int>;
}
