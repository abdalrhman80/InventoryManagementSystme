using InventoryManagement.Application.Transactions.Command.CreateTransaction;
using InventoryManagement.Application.Transactions.Command.CancelTransaction;
using InventoryManagement.Application.Transactions.Command.UpdateTransaction;
using InventoryManagement.Application.Transactions.Query.GetAllTransactions;
using InventoryManagement.Application.Transactions.Query.GetTransaction;
using InventoryManagement.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    [Authorize]
    public class TransactionController(IMediator mediator) : ControllerBase
    {
        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] GetAllTransactionsQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var result = await mediator.Send(new GetTransactionQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetTransaction), new { id }, null);
        }

        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("cancel/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id, CancelTransactionCommand command)
        {
            command.Id = id;
            await mediator.Send(command);
            return NoContent();
        }
    }
}
