using InventoryManagement.Application.LowStockAlerts.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace InventoryManagement.Application.LowStockAlerts.Command.UpdateLowStockAlert
{
    public class UpdateLowStockAlertCommand : IRequest<LowStockAlertDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Threshold { get; set; }
    }
}
