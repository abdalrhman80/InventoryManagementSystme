using InventoryManagement.Application.Transactions.DTOs;
using InventoryManagement.Domain.Constants;
using MediatR;
using System.Text.Json.Serialization;

namespace InventoryManagement.Application.Transactions.Command.UpdateTransaction
{
    public class UpdateTransactionCommand : IRequest<TransactionDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public TransactionType? TransactionType { get; set; }
        public int? NewQuantity { get; set; }
    }
}
