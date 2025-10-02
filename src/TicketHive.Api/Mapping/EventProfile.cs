namespace TicketHive.Api.Mapping;
using AutoMapper;
using TicketHive.Api.Contracts.Events;
using TicketHive.Application.Events;
using TicketHive.Application.Events.Command;
using TicketHive.Application.Common;

public class EventProfile : Profile
{
    public EventProfile()
    {
        // Create event mappings
        CreateMap<AddEventRequest, AddEventCommand>()
            .ConstructUsing(src => new AddEventCommand(
                src.Name,
                src.Slug,
                src.Description,
                src.Location,
                src.StartTime,
                src.EndTime,
                src.VenueCapacity,
                src.SaleStartTime,
                src.SaleEndTime,
                src.ImageUrl,
                src.IsFeatured,
                Guid.Empty
            ));

        CreateMap<AddEventResult, AddEventResponse>();

        // Get event mappings
        CreateMap<EventDetailResult, GetEventResponse>();
        CreateMap<EventResult, EventSummary>();
        CreateMap<List<EventResult>, GetEventsResponse>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src));

        // Paged events mappings
        CreateMap<GetPagedEventsRequest, GetPagedEventsQuery>();
        CreateMap<PagedResult<EventResult>, GetPagedEventsResponse>()
            .ForPath(dest => dest.Events.Items, opt => opt.MapFrom(src => src.Items))
            .ForPath(dest => dest.Events.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForPath(dest => dest.Events.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForPath(dest => dest.Events.PageSize, opt => opt.MapFrom(src => src.PageSize));

        // Update event mappings
        CreateMap<UpdateEventRequest, UpdateEventCommand>()
            .ForMember(dest => dest.EventId, opt => opt.Ignore());
        CreateMap<EventDetailResult, UpdateEventResponse>();

        // Publish event mappings
        CreateMap<PublishEventResult, EventResponse>();
        CreateMap<PublishEventRequest, PushlishEventCommand>()
            .ConstructUsing(src => new PushlishEventCommand(
                src.EventId
            ));
    }
}