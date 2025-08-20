using AutoMapper;
using InventoryManagement.Application.Account.DTOs;
using InventoryManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace InventoryManagement.Application.Account.Resolver
{
    internal class UserImageUrlResolver(IConfiguration _configuration) : IValueResolver<User, UserDto, string>
    {
        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
            => !string.IsNullOrEmpty(source.ImagePath) ? $"{_configuration["BaseUrl"]}/{source.ImagePath}" : null!;
    }
}
