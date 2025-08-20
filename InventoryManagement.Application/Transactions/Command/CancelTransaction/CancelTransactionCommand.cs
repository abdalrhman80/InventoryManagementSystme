using MediatR;
using System.Text.Json.Serialization;

namespace InventoryManagement.Application.Transactions.Command.CancelTransaction
{
    public class CancelTransactionCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Reason { get; set; }
    }
}
