using AutoMapper;
using Blessings.Domain;
using Blessings.Order.Core.CQRS.Queries;
using Blessings.OrdersApi.Models;

namespace Blessings.OrdersApi.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Set, SetResponse>();
        CreateMap<Domain.Order, OrderResponse>();
        CreateMap<Domain.Order, OrderDto>();
        CreateMap<Domain.Order, OrderStatusDto>()
            .ForMember(dest => dest.OrderId,
                opt => opt.MapFrom(src => src.Id));
    }
}