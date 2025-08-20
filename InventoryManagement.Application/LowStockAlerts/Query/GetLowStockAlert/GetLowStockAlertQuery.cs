using InventoryManagement.Application.LowStockAlerts.DTOs;
using MediatR;

namespace InventoryManagement.Application.LowStockAlerts.Query.GetLowStockAlert
{
    public record GetLowStockAlertQuery(int Id) : IRequest<LowStockAlertDto>;
}
