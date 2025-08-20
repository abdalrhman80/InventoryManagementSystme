using AutoMapper;
using InventoryManagement.Application.Categories.Commands.CreateCategory;
using InventoryManagement.Application.Categories.DTOs;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Categories.Profiles
{
    internal class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Description, src => src.MapFrom(src => src.Description ?? string.Empty));

            CreateMap<CreateCategoryCommand, Category>()
                .ForMember(dest => dest.Name, src => src.MapFrom(src => src.CategoryName));
        }
    }
}
