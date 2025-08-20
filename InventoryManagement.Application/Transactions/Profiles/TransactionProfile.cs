using AutoMapper;
using InventoryManagement.Application.Transactions.Command.CreateTransaction;
using InventoryManagement.Application.Transactions.Command.UpdateTransaction;
using InventoryManagement.Application.Transactions.DTOs;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Transactions.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdateDate ?? null))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            CreateMap<CreateTransactionCommand, Transaction>()
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"))))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));

            CreateMap<UpdateTransactionCommand, Transaction>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TransactionType));
        }
    }
}
