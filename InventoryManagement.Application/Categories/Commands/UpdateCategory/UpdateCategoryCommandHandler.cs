using AutoMapper;
using InventoryManagement.Application.Categories.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler(
        ILogger<UpdateCategoryCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("Admin {AdminId} is updating category {CategoryId}", currentUser.Id, request.CategoryId);

            var dbCategory = await unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId)
                ?? throw new NotFoundException($"Category with ID {request.CategoryId} not found");

            dbCategory.Name = request.CategoryName ?? dbCategory.Name;
            dbCategory.Description = request.Description ?? dbCategory.Description;

            if (string.IsNullOrEmpty(dbCategory.Name))
                throw new BadRequestException("Category name cannot be null or empty");

            unitOfWork.Repository<Category>().Update(dbCategory);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<CategoryDto>(dbCategory);
        }
    }
}
