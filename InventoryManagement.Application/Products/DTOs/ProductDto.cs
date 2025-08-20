namespace InventoryManagement.Application.Products.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Supplier { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
