using InventoryManagement.Application.Account.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InventoryManagement.Application.Account.Commands.UploadProfileImage
{
    public record UploadProfileImageCommand(IFormFile Image) : IRequest<UserDto>;
}
