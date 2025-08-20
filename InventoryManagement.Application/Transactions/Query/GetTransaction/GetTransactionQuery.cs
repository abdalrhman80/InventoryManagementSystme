using InventoryManagement.Application.Transactions.DTOs;
using MediatR;

namespace InventoryManagement.Application.Transactions.Query.GetTransaction
{
    public record GetTransactionQuery(int Id) : IRequest<TransactionDto>;
}
