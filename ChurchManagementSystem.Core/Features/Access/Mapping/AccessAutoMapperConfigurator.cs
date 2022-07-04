using ChurchManagementSystem.Core.Features.Access.Dtos;
using AutoMapper;
using IdentityModel.Client;

namespace ChurchManagementSystem.Core.Features.Access.Mapping
{
    public class AccessAutoMapperConfigurator : Profile
    {
        public AccessAutoMapperConfigurator()
        {
            CreateMap<TokenResponse, TokenDto>();
        }
    }
}