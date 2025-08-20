namespace InventoryManagement.Domain.Entities
{
    public class LowStockAlert
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Threshold { get; set; }
        public bool AlertSent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? LastAlertSent { get; set; }

        public Product Product { get; set; }
    }
}
