using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler(
        ILogger<DeleteProductCommandHandler> _logger,
        IUserContext _userContext,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<DeleteProductCommand>
    {
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("User {UserId} is deleting product {ProductId}", currentUser.Id, request.Id);

            var dbProduct = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(message: $"Product with ID {request.Id} not found.");

            _unitOfWork.ProductRepository.Delete(dbProduct);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
