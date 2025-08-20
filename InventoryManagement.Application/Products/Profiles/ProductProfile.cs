using AutoMapper;
using InventoryManagement.Application.Products.Commands.CreateProduct;
using InventoryManagement.Application.Products.Commands.UpdateProduct;
using InventoryManagement.Application.Products.DTOs;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Products.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateProductCommand, Product>();
        }
    }
}
