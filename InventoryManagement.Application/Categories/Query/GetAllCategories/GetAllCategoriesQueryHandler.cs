using AutoMapper;
using InventoryManagement.Application.Categories.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Categories.Query.GetAllCategories
{
    public class GetAllCategoriesQueryHandler(
        ILogger<GetAllCategoriesQueryHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetAllCategoriesQuery, PaginationResponse<CategoryDto>>
    {
        public async Task<PaginationResponse<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is requesting all categories", currentUser.Id);

            var categorySpec = new CategoriesSpecifications(request.PageSize, request.PageNumber);

            var dbCategories = await unitOfWork.Repository<Category>().GetAllWithSpecificationAsync(categorySpec);

            if (dbCategories == null || !dbCategories.Any())
                return new PaginationResponse<CategoryDto>([], 0, request.PageNumber, request.PageSize);

            var categories = mapper.Map<IReadOnlyList<CategoryDto>>(dbCategories);

            var paginationResponse = new PaginationResponse<CategoryDto>(
                categories,
                dbCategories.Count,
                request.PageNumber,
                request.PageSize
            );

            return paginationResponse;
        }
    }
}
