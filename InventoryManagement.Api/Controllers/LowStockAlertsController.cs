using InventoryManagement.Application.LowStockAlerts.Command.CreateLowStockAlert;
using InventoryManagement.Application.LowStockAlerts.Command.UpdateLowStockAlert;
using InventoryManagement.Application.LowStockAlerts.Query.GetLowStockAlert;
using InventoryManagement.Application.LowStockAlerts.Query.GetLowStockAlertByProcut;
using InventoryManagement.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/lowstockalerts")]
    [ApiController]
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
    public class LowStockAlertsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateLowStockAlert(CreateLowStockAlertCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAlertById), new { id }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlertById(int id)
        {
            var result = await _mediator.Send(new GetLowStockAlertQuery(id));
            return Ok(result);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetAlertByProductId(int id)
        {
            var result = await _mediator.Send(new GetLowStockAlertByProcutQuery(id));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLowStockAlert(int id, UpdateLowStockAlertCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
