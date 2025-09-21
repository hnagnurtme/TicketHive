namespace TicketHive.Api.Mapping;
using AutoMapper;
using TicketHive.Api.Contracts.Events;
using TicketHive.Application.Events;
using TicketHive.Application.Events.Command;

public class EventProfile : Profile
{
    public EventProfile()
    {
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
    }
}