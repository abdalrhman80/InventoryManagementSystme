using InventoryManagement.Application.Products.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace InventoryManagement.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        //public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Supplier { get; set; }
    }
}
