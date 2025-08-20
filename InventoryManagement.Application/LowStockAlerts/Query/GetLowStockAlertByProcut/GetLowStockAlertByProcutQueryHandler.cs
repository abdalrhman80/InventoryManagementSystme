using AutoMapper;
using InventoryManagement.Application.LowStockAlerts.DTOs;
using InventoryManagement.Application.LowStockAlerts.Query.GetLowStockAlert;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.LowStockAlerts.Query.GetLowStockAlertByProcut
{
    public class GetLowStockAlertByProcutQueryHandler(
    ILogger<GetLowStockAlertByProcutQueryHandler> logger,
    IUserContext userContext,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<GetLowStockAlertByProcutQuery, LowStockAlertDto>
    {
        public async Task<LowStockAlertDto> Handle(GetLowStockAlertByProcutQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is request to get low stock alert for product {ProductId}", currentUser.Id, request.ProductId);

            var lowStockAlertSpec = new LowStockAlertSpecification(productId: request.ProductId, includeProduct: true);
            var existingAlert = await unitOfWork.Repository<LowStockAlert>().GetEntityWithSpecificationAsync(lowStockAlertSpec)
                ?? throw new NotFoundException("No Alert Found");

            var result = mapper.Map<LowStockAlertDto>(existingAlert);

            return result;
        }
    }
}
