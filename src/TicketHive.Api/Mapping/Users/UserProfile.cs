using AutoMapper;
using TicketHive.Api.Contracts.Users;
using TicketHive.Application.Users.Query;
namespace TicketHive.Api.Mapping.Users;


public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserProfileResult, UserProfileResponse>();
    }
}


