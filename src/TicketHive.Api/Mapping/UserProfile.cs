using AutoMapper;
using TicketHive.Api.Contracts.Users;
using TicketHive.Application.Users.Command;
using TicketHive.Application.Users.Result;
namespace TicketHive.Api.Mapping.Users;


public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserProfileResult, UserProfileResponse>();
        CreateMap<UpdatedUserProfileResult, UpdateUserProfileResponse>();
        CreateMap<UpdateUserProfileRequest, UpdateUserProfileCommand>();
        
    }
}


