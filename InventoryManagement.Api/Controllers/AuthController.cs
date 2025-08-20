using InventoryManagement.Application.Auth.Commands.ConfirmEmail;
using InventoryManagement.Application.Auth.Commands.Login;
using InventoryManagement.Application.Auth.Commands.Register;
using InventoryManagement.Application.Auth.Commands.ResendConfirmation;
using InventoryManagement.Application.Auth.Commands.ForgetPassword;
using InventoryManagement.Application.Auth.Commands.ResetPassword;
using InventoryManagement.Application.Auth.Queries.RefreshTokens;
using InventoryManagement.Application.Auth.Commands.RevokeToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("email-confirmation")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }

        [HttpGet("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] RefreshTokenQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromQuery] RevokeTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = result });
        }
    }
}
