using AutoMapper;
using OrderAddress = Domain.Models.OrderModels.Address;
using UserAddress = Domain.Identity.Address;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.OrderModels;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderResultDto>()
                    .ForMember(d => d.PaymentSatuts, o => o.MapFrom(s => s.PaymentSatuts.ToString()))
                    .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                    .ForMember(d => d.Total, o => o.MapFrom(s => s.SubTotal + s.DeliveryMethod.Cost));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();


            CreateMap<OrderItem, OrderItemDto>()
                    .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                    .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                    .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl));

            CreateMap<OrderAddress, AddressDto>().ReverseMap();
            CreateMap<UserAddress, AddressDto>().ReverseMap();


        }
    }
}
