using AutoMapper;
using InventoryManagement.Application.Products.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler(
        ILogger<UpdateProductCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} is updating product{ProductId} with values {@UpdatedProduct}", currentUser.Id, request.Id, request);

            var productSpec = new ProductSpecifications(request.Id);

            var dbProduct = await _unitOfWork.ProductRepository.GetEntityWithSpecificationAsync(productSpec)
                ?? throw new NotFoundException($"Product with ID {request.Id} not found.");

            //_ = await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId)
            //    ?? throw new NotFoundException(message: $"Category with ID {request.CategoryId} not found.");

            //var dbProduct = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id)
            //    ?? throw new NotFoundException(message: $"Product with ID {request.Id} not found.");

            dbProduct.Name = request.Name ?? dbProduct.Name;
            dbProduct.Description = request.Description ?? dbProduct.Description;
            dbProduct.Price = request.Price;
            dbProduct.StockQuantity = request.StockQuantity;
            dbProduct.Supplier = request.Supplier ?? dbProduct.Supplier;

            _unitOfWork.ProductRepository.Update(dbProduct);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(dbProduct);
        }
    }
}
