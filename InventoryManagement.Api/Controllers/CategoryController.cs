using InventoryManagement.Application.Categories.Commands.CreateCategory;
using InventoryManagement.Application.Categories.Commands.DeleteCategory;
using InventoryManagement.Application.Categories.Commands.UpdateCategory;
using InventoryManagement.Application.Categories.Query.GetAllCategories;
using InventoryManagement.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize(Roles = RoleNames.Admin)]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] GetAllCategoriesQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetCategories), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command)
        {
            command.CategoryId = id;
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}
