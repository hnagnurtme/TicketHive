using AutoMapper;
using TicketHive.Api.Contracts.Tickets;
using TicketHive.Application.Common;
using TicketHive.Application.Tickets;
using TicketHive.Domain.Entities;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        // Create ticket mappings
        CreateMap<AddTicketRequest, CreateTicketCommand>();
        CreateMap<TicketResult, AddTicketResponse>();
        
        // Get ticket mappings
        CreateMap<TicketDetailResult, GetTicketResponse>();
        CreateMap<TicketResult, TicketSummary>();
        CreateMap<List<TicketResult>, GetTicketsByEventResponse>()
            .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src));

        // Update ticket mappings
        CreateMap<UpdateTicketRequest, UpdateTicketCommand>()
            .ForMember(dest => dest.TicketId, opt => opt.Ignore());
        CreateMap<TicketDetailResult, UpdateTicketResponse>();
        
        // Pagination mappings
        CreateMap<GetTicketsRequest, GetTicketsQuery>();
        CreateMap<PagedResult<TicketResult>, GetTicketsResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Items = src.Items?.Select(t => new TicketSummary
                {
                    Id = t.TicketId,
                    Type = t.Type,
                    Name = t.Name,
                    Price = t.Price,
                    TotalQuantity = t.TotalQuantity,
                    RemainingQuantity = t.RemainingQuantity,
                    MinPurchase = t.MinPurchase,
                    MaxPurchase = t.MaxPurchase,
                    Description = t.Description,
                    OriginalPrice = t.OriginalPrice,
                    SaleStartTime = t.SaleStartTime,
                    SaleEndTime = t.SaleEndTime,
                    IsActive = t.IsActive,
                    SortOrder = t.SortOrder
                }) ?? Enumerable.Empty<TicketSummary>();
                dest.TotalItems = src.TotalItems;
                dest.PageNumber = src.PageNumber;
                dest.PageSize = src.PageSize;
            });
    }
}