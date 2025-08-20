using MediatR;

namespace InventoryManagement.Application.LowStockAlerts.Command.CreateLowStockAlert
{
    public record CreateLowStockAlertCommand(int ProductId, int Threshold) : IRequest<int>;
}
