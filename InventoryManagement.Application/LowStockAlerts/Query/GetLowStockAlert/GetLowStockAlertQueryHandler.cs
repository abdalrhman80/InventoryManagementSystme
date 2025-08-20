using AutoMapper;
using InventoryManagement.Application.LowStockAlerts.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.LowStockAlerts.Query.GetLowStockAlert
{
    public class GetLowStockAlertQueryHandler(
        ILogger<GetLowStockAlertQueryHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<GetLowStockAlertQuery, LowStockAlertDto>
    {
        public async Task<LowStockAlertDto> Handle(GetLowStockAlertQuery request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is request to get low stock alert {StockAlertId}", currentUser.Id, request.Id);

            var lowStockAlertSpec = new LowStockAlertSpecification(id: request.Id, includeProduct: true);
            var existingAlert = await unitOfWork.Repository<LowStockAlert>().GetEntityWithSpecificationAsync(lowStockAlertSpec)
                ?? throw new NotFoundException("No Alert Found");

            var result = mapper.Map<LowStockAlertDto>(existingAlert);

            return result;
        }
    }
}
