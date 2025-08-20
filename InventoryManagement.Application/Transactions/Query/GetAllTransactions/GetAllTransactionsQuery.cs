using InventoryManagement.Application.Transactions.DTOs;
using InventoryManagement.Domain.Common;
using InventoryManagement.Domain.Constants;
using MediatR;

namespace InventoryManagement.Application.Transactions.Query.GetAllTransactions
{
    public class GetAllTransactionsQuery : IRequest<PaginationResponse<TransactionDto>>
    {
        private int pageSize = 5;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 10 ? 10 : value;
        }

        public int PageNumber { get; set; } = 1;
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
        public TransactionType? TransactionType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
