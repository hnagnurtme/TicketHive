using AutoMapper;
using TicketHive.Api.Contracts.Authentication;
using TicketHive.Application.Authentication;
namespace TicketHive.Api.Mapping.Authentication;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<LoginRequest, LoginQuery>();
        CreateMap<AuthenticationResult, AuthenticationResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken != null 
                ? src.RefreshToken.RefreshToken 
                : string.Empty));
        CreateMap<RefreshTokenResult, RefreshTokenResponse>();
    }
}