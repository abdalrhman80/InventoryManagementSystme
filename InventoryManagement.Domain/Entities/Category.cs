using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Product> Products { get; set; } = [];
    }
}
