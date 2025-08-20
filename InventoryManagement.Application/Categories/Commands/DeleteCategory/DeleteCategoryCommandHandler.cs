using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler(
        ILogger<DeleteCategoryCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<DeleteCategoryCommand>
    {
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("Admin {AdminId} is deleting category {CategoryId}", currentUser.Id, request.CategoryId);

            var dbCategory = await unitOfWork.Repository<Category>().GetEntityWithSpecificationAsync(new CategoriesSpecifications(request.CategoryId))
               ?? throw new NotFoundException($"Category with ID {request.CategoryId} not found");

            if (dbCategory.Products.Count != 0)
                throw new BadRequestException("Cannot delete category with existing products");

            unitOfWork.Repository<Category>().Delete(dbCategory);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
