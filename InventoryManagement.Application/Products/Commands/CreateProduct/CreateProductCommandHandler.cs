using AutoMapper;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler(
        ILogger<CreateProductCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IEmailService _emailService
        ) : IRequestHandler<CreateProductCommand, int>
    {
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} is creating a product {@Product}", currentUser.Id, request);

            _ = await _unitOfWork.Repository<Category>().GetByIdAsync(request.CategoryId)
                ?? throw new NotFoundException(message: $"Category with id {request.CategoryId} not found.");

            var existingProduct = await _unitOfWork.ProductRepository.GetByNameAsync(request.Name);

            if (existingProduct != null)
                throw new BadRequestException($"Product {request.Name} already exist.");

            var product = _mapper.Map<Product>(request);
            product.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));

            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // await _emailService.SendProductAddedConfirmationAsync(currentUser.Email, currentUser.UserName, product);

            return product.Id;
        }
    }
}
