namespace InventoryManagement.Application.LowStockAlerts.DTOs
{
    public class LowStockAlertDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int CurrentStock { get; set; }
        public int Threshold { get; set; }
        public bool AlertSent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? LastAlertSent { get; set; }
    }
}
