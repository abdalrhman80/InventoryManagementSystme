using InventoryManagement.Domain.Constants;

namespace InventoryManagement.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int ProductId { get; set; }
        public string CreatedBy { get; set; }
        public string? Status { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? CancelledBy { get; set; }
        public string? CancellationReason { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}
