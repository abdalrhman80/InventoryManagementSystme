using AutoMapper;
using InventoryManagement.Application.LowStockAlerts.DTOs;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.LowStockAlerts.Profiles
{
    public class LowStockAlertProfile : Profile
    {
        public LowStockAlertProfile()
        {
            CreateMap<LowStockAlert, LowStockAlertDto>()
                .ForMember(dest => dest.ProductName, src => src.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.CurrentStock, src => src.MapFrom(src => src.Product.StockQuantity))
                .ForMember(dest => dest.Threshold, src => src.MapFrom(src => src.Threshold))
                .ForMember(dest => dest.CreatedAt, src => src.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.SentAt, src => src.MapFrom(src => src.SentAt))
                .ForMember(dest => dest.LastAlertSent, src => src.MapFrom(src => src.LastAlertSent));
        }
    }
}
