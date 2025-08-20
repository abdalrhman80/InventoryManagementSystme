using AutoMapper;
using InventoryManagement.Application.Auth.Commands.Register;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Auth.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterCommand, User>();

        }
    }
}
