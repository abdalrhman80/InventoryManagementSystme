using InventoryManagement.Domain.Constants;
using MediatR;

namespace InventoryManagement.Application.Transactions.Command.CreateTransaction
{
    public record CreateTransactionCommand(int ProductId, int Quantity, TransactionType TransactionType) : IRequest<int>;
}
