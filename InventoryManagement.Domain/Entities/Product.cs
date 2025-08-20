using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        [MaxLength(100)]
        public string Supplier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
