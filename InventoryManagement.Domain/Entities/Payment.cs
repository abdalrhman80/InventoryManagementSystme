namespace InventoryManagement.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // e.g., "Credit Card", "Cash", etc.
        public string PaymentStatus { get; set; }

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}
