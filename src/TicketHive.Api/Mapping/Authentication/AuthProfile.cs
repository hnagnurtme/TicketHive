using AutoMapper;
using TicketHive.Api.Contracts.Authentication;
using TicketHive.Application.Authentication;
using TicketHive.Application.Queries.Auth;
namespace TicketHive.Api.Mapping.Authentication;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<LoginRequest, LoginQuery>();
        CreateMap<AuthenticationResult, AuthenticationResponse>();
    }
}