using AutoMapper;

using ChurchManagementSystem.Web.Extentions;
using ChurchManagementSystem.Web.Services.Users.Models;

namespace ChurchManagementSystem.Web.Services.Users
{
    public class UsersAutoMapperConfiguration : Profile
    {
        public UsersAutoMapperConfiguration()
        {
            CreateMap<CreateUserRequestDto, CreateUserRequest>()
                .ForMember(x => x.Active, options => options.MapFrom(s => true));

        }
    }
}