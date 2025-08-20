using AutoMapper;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(
        ILogger<CreateCategoryCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<CreateCategoryCommand, int>
    {
        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("Admin {AdminId} is creating a new category", currentUser.Id);

            if (string.IsNullOrEmpty(request.CategoryName))
                throw new BadRequestException("Category name cannot be null or empty");

            var category = mapper.Map<Category>(request);

            await unitOfWork.Repository<Category>().AddAsync(category);
            await unitOfWork.SaveChangesAsync();

            return category.Id;
        }
    }
}
