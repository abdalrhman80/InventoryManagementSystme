namespace InventoryManagement.Application.Transactions.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        //public string UserId { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TransactionType { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
