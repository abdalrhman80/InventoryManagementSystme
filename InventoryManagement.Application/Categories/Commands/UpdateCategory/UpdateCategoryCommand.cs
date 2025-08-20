using InventoryManagement.Application.Categories.DTOs;
using MediatR;
using System.Text.Json.Serialization;

namespace InventoryManagement.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<CategoryDto>
    {
        [JsonIgnore]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
