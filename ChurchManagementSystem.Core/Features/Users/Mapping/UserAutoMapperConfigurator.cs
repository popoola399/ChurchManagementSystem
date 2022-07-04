using System;
using System.Linq;
using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.Users.Dtos;
using ChurchManagementSystem.Core.Security;
using AutoMapper;


namespace ChurchManagementSystem.Core.Features.Users.Mapping
{
    public class UserAutoMapperConfigurator : Profile
    {
        public UserAutoMapperConfigurator()
        {
            CreateMap<AddUserRequest, User>()
                .ForMember(x => x.CreatedDate, options => options.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.ModifiedDate, options => options.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.CannotChangePassword, options => options.MapFrom(s => false));

        }
    }
}