using AutoMapper;
using InventoryManagement.Application.Account.DTOs;
using InventoryManagement.Application.Account.Resolver;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Account.Profiles
{
    public class AccountUserProfile : Profile
    {
        public AccountUserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name).ToList()))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom<UserImageUrlResolver>());
        }
    }
}
