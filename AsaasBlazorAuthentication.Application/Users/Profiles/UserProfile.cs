using AutoMapper;

using AsaasBlazorAuthentication.Application.Users.UpdateUser;

namespace AsaasBlazorAuthentication.Application.Users.Profiles;

internal sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UpdateUserInputModel, UpdateUserCommand>();
    }
}