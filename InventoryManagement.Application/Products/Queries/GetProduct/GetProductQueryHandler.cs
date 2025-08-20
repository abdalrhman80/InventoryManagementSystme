using AutoMapper;
using InventoryManagement.Application.Products.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Queries.GetProduct
{
    public class GetProductQueryHandler(
        ILogger<GetProductQueryHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<GetProductQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} is retrieving product with id {ProductId}", currentUser.Id, request.Id);

            var productSpec = new ProductSpecifications(request.Id);

            var product = await _unitOfWork.ProductRepository.GetEntityWithSpecificationAsync(productSpec)
                ?? throw new NotFoundException(message: $"Product with id {request.Id} not found.");

            var result = _mapper.Map<ProductDto>(product);

            return result;
        }
    }

}
