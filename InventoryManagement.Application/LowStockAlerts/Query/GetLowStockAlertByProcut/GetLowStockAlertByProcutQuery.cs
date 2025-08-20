using InventoryManagement.Application.LowStockAlerts.DTOs;
using MediatR;

namespace InventoryManagement.Application.LowStockAlerts.Query.GetLowStockAlertByProcut
{
    public record GetLowStockAlertByProcutQuery(int ProductId) : IRequest<LowStockAlertDto>;
}
