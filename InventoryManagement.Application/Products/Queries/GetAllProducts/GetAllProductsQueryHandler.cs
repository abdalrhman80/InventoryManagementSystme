using AutoMapper;
using InventoryManagement.Application.Products.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler(
        ILogger<GetAllProductsQueryHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetAllProductsQuery, PaginationResponse<ProductDto>>
    {
        public async Task<PaginationResponse<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} listing all products with parameters {@Params}", currentUser.Id, request);

            var productSpec = new ProductSpecifications(request.PageNumber, request.PageSize, request.SortBy, request.SortDirection, request.CategoryName);

            var dbProducts = await _unitOfWork.ProductRepository.GetAllWithSpecificationAsync(productSpec);

            if (dbProducts == null || !dbProducts.Any())
                return new PaginationResponse<ProductDto>([], 0, request.PageSize, request.PageNumber);

            var products = _mapper.Map<IReadOnlyList<ProductDto>>(dbProducts);

            var paginationResponse = new PaginationResponse<ProductDto>(products, products.Count, request.PageSize, request.PageNumber);

            return paginationResponse;
        }
    }
}
