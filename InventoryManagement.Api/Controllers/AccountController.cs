using InventoryManagement.Application.Account.Commands.AddUserToRole;
using InventoryManagement.Application.Account.Commands.ChangePassword;
using InventoryManagement.Application.Account.Commands.DeleteProfileImage;
using InventoryManagement.Application.Account.Commands.DeleteUser;
using InventoryManagement.Application.Account.Commands.RemoveUserFromRole;
using InventoryManagement.Application.Account.Commands.UpdateUser;
using InventoryManagement.Application.Account.Commands.UploadProfileImage;
using InventoryManagement.Application.Account.Queries.GetAllUsers;
using InventoryManagement.Application.Account.Queries.GetUser;
using InventoryManagement.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController(IMediator _mediator) : ControllerBase
    {
        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("admin/users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var result = await _mediator.Send(new GetUserQuery());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            await _mediator.Send(new DeleteUserCommand());
            return NoContent();
        }

        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadProfileImageCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("image")]
        public async Task<IActionResult> DeleteImage()
        {
            await _mediator.Send(new DeleteProfileImageCommand());
            return NoContent();
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("admin/role")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserToRoleCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("admin/role")]
        public async Task<IActionResult> RemoveUserFromRole([FromBody] RemoveUserFromRoleCommand request)
        {
            await _mediator.Send(request);
            return NoContent();
        }
    }
}
