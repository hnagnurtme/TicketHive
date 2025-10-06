using AutoMapper;
using TicketHive.Api.Contracts.Orders;
using TicketHive.Application.Orders.Commands.CreateOrder;
using TicketHive.Application.Orders.Result;
using TicketHive.Domain.Entities; 

namespace TicketHive.Application.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItemRequest, CreateOrderItem>();
        CreateMap<CreateOrderRequest, CreateOrderCommand>()
            .ConstructUsing(src => new CreateOrderCommand(
                Guid.Empty, 
                src.Items.Select(i => new CreateOrderItem(i.TicketId, i.Quantity)).ToList(),
                src.PaymentProvider,
                src.CouponCode 
            ));
        CreateMap<OrderItem, OrderItemResult>()
            .ForMember(dest => dest.TicketName, opt => opt.MapFrom(src => src.Ticket.Name)); 
        CreateMap<Order, OrderResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) 
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        CreateMap<OrderItemResult, OrderItemResponse>();
        CreateMap<OrderResult, CreateOrderResponse>();
    }
}