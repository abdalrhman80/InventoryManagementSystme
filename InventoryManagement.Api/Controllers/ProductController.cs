using InventoryManagement.Application.Products.Commands.CreateProduct;
using InventoryManagement.Application.Products.Commands.UpdateProduct;
using InventoryManagement.Application.Products.Commands.DeleteProduct;
using InventoryManagement.Application.Products.Queries.GetAllProducts;
using InventoryManagement.Application.Products.Queries.GetProduct;
using InventoryManagement.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _mediator.Send(new GetProductQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
        {
            var id = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetProduct), new { id }, null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand request)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
    }
}
