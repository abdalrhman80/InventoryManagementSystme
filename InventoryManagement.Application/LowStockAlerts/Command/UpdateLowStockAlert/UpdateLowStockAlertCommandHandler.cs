using AutoMapper;
using InventoryManagement.Application.LowStockAlerts.DTOs;
using InventoryManagement.Application.UserContextService;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Repositories;
using InventoryManagement.Domain.Specifications.EntitiesSpecifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Application.LowStockAlerts.Command.UpdateLowStockAlert
{
    public class UpdateLowStockAlertCommandHandler(
        ILogger<UpdateLowStockAlertCommandHandler> logger,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) : IRequestHandler<UpdateLowStockAlertCommand, LowStockAlertDto>
    {
        public async Task<LowStockAlertDto> Handle(UpdateLowStockAlertCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            logger.LogInformation("User {UserId} is update the low stock alert threshold {LowStockAlertThreshold}", currentUser.Id, request.Threshold);

            var alertSpec = new LowStockAlertSpecification(id: request.Id, includeProduct: true);

            var alert = await unitOfWork.Repository<LowStockAlert>().GetEntityWithSpecificationAsync(alertSpec)
                ?? throw new NotFoundException("No alert found!");

            alert.Threshold = request.Threshold;
            alert.AlertSent = false;

            unitOfWork.Repository<LowStockAlert>().Update(alert);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<LowStockAlertDto>(alert);
        }
    }
}
