using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.LowStockAlerts.Command.CreateLowStockAlert
{
    public class CreateLowStockAlertCommandHandler(
        ILogger<CreateLowStockAlertCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<CreateLowStockAlertCommand, int>
    {
        public async Task<int> Handle(CreateLowStockAlertCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is create low stock alert {@LowStockAlert}", currentUser.Id, request);

            _ = await unitOfWork.ProductRepository.GetByIdAsync(request.ProductId) ?? throw new NotFoundException("Product not found");

            var lowStockAlertSpec = new LowStockAlertSpecification(productId: request.ProductId);
            var existingAlert = await unitOfWork.Repository<LowStockAlert>().GetEntityWithSpecificationAsync(lowStockAlertSpec);

            if (existingAlert != null)
                throw new BadRequestException("Alert already exists for this product");

            var alert = new LowStockAlert
            {
                ProductId = request.ProductId,
                Threshold = request.Threshold,
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"))
            };

            await unitOfWork.Repository<LowStockAlert>().AddAsync(alert);
            await unitOfWork.SaveChangesAsync();

            return alert.Id;
        }
    }
}
