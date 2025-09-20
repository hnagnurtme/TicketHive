using AutoMapper;
using TicketHive.Api.Contracts.Authentication;
using TicketHive.Application.Authentication;
using TicketHive.Application.Authentication.Commands.RefreshToken;
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
        CreateMap<RefreshTokenRequest, ValidateRefreshTokenCommand>()
            .ForCtorParam("UserId", opt => opt.MapFrom(src => src.UserId))
            .ForCtorParam("RefreshToken", opt => opt.MapFrom(src => src.Token))
            .ForCtorParam("IpAddress", opt => opt.MapFrom(src => src.IpAddress))
            .ForCtorParam("UserAgent", opt => opt.MapFrom(src => src.UserAgent))
            .ForCtorParam("DeviceFingerprint", opt => opt.MapFrom(src => src.DeviceFingerprint ?? string.Empty))
    }
}