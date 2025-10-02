using AutoMapper;
using TicketHive.Api.Contracts.Tickets;
using TicketHive.Application.Tickets;
using TicketHive.Domain.Entities;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<AddTicketRequest, CreateTicketCommand>();
        CreateMap<TicketResult, AddTicketResponse>();
    }
}